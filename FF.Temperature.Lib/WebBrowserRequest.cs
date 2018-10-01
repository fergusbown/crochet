using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FF.Temperature.Lib
{
    internal delegate bool IsDocumentFullyLoaded(HtmlAgilityPack.HtmlDocument htmlDocument, out int remaining);

    internal class WebBrowserRequest
    {
        private const int maxSilentAttempts = 2;

        private readonly WebBrowser browser;
        private readonly IsDocumentFullyLoaded isDocumentFullyLoaded;
        private readonly string url;
        private readonly IUserInteraction userInteraction;
        private readonly ManualResetEvent manualResetEvent;

        private readonly object lockObject;

        private readonly HtmlAgilityPack.HtmlDocument htmlDocument;

        private int lastRemaining;
        private int lastCompletedCount;
        private int completedCount;
        private int numberOfAttempts;
        private bool finished;

        private string description => this.url;

        private WebBrowserRequest(string url, IsDocumentFullyLoaded isDocumentFullyLoaded, IUserInteraction userInteraction)
        {
            this.browser = new WebBrowser();
            this.browser.ScriptErrorsSuppressed = true;
            this.browser.DocumentCompleted += Browser_DocumentCompleted;
            this.isDocumentFullyLoaded = isDocumentFullyLoaded;
            this.url = url;
            this.userInteraction = userInteraction;

            this.manualResetEvent = new ManualResetEvent(false);
            this.htmlDocument = new HtmlAgilityPack.HtmlDocument();
            this.lockObject = new object();
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            RefreshDocument(completing:true);
        }

        private void Finish(string finishType)
        {
            this.finished = true;
            this.userInteraction.Log($"{finishType} '{this.description}'");
            this.browser.Stop();
            this.manualResetEvent.Set();
        }

        private void RefreshDocument(bool completing)
        {
            if (this.browser.InvokeRequired)
            {
                Action action = () => this.RefreshDocument(completing);
                this.browser.Invoke(action);
            }
            else
            {
                if (finished)
                    return;

                string html = this.browser.Document?.Body?.OuterHtml ?? string.Empty;

                this.htmlDocument.LoadHtml(html);

                int remaining;

                if (this.isDocumentFullyLoaded(this.htmlDocument, out remaining))
                {
                    Finish("Finished");
                    return;
                }

                this.userInteraction.Log($"{remaining} readings remaining for '{this.description}'");

                if (completing)
                {
                    completedCount++;
                    return;
                }

                if (completedCount != lastCompletedCount)
                {
                    lastCompletedCount = completedCount;
                    return;
                }

                if (this.lastRemaining == remaining)
                {
                    this.lastRemaining = 0;
                    this.numberOfAttempts++;

                    bool tryAgain = (this.numberOfAttempts % maxSilentAttempts != 0) 
                        || this.userInteraction.ShouldContinue($"{numberOfAttempts} attempts made to load '{this.description}' - try again?");

                    if (tryAgain)
                    {
                        this.userInteraction.Log($"Attempting '{this.description}' again");
                        this.browser.Navigate(this.url);
                    }
                    else
                    {
                        Finish("Cancelled");
                        return;
                    }
                }
                else
                {
                    this.lastRemaining = remaining;
                }
            }
        }

        private async Task<HtmlAgilityPack.HtmlDocument> Navigate()
        {
            this.userInteraction.Log($"Processing '{this.description}'");

            this.manualResetEvent.Reset();
            this.browser.Navigate(this.url);

            await Task.Factory.StartNew(() =>
            {
                int completedCount = 0;
                while (!this.manualResetEvent.WaitOne(2000))
                {
                    if (completedCount == lastCompletedCount)
                    {
                        RefreshDocument(completing: false);
                    }
                    else
                    {
                        completedCount = lastCompletedCount;
                    }
                }
            });

            return this.htmlDocument;
        }

        public static async Task<HtmlAgilityPack.HtmlDocument> Navigate(
            string url, 
            IsDocumentFullyLoaded isDocumentFullyLoaded,
            IUserInteraction userInteraction)
        {
            var request = new WebBrowserRequest(url, isDocumentFullyLoaded, userInteraction);
            return await request.Navigate();
        }
    }
}
