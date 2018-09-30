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
        private readonly WebBrowser browser;
        private readonly IsDocumentFullyLoaded isDocumentFullyLoaded;
        private readonly string url;
        private readonly ManualResetEvent manualResetEvent;

        private readonly HtmlAgilityPack.HtmlDocument htmlDocument;

        private int lastRemaining;
        private int lastCompletedCount;
        private int completedCount;

        private WebBrowserRequest(string url, IsDocumentFullyLoaded isDocumentFullyLoaded)
        {
            this.browser = new WebBrowser();
            this.browser.ScriptErrorsSuppressed = true;
            this.browser.DocumentCompleted += Browser_DocumentCompleted;
            this.isDocumentFullyLoaded = isDocumentFullyLoaded;
            this.url = url;

            this.manualResetEvent = new ManualResetEvent(false);
            this.htmlDocument = new HtmlAgilityPack.HtmlDocument();
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            RefreshDocument(completing:true);
        }

        private void RefreshDocument(bool completing)
        {
            if (this.browser.InvokeRequired)
            {
                Action action = () => this.RefreshDocument(completing);
                this.browser.BeginInvoke(action);
            }
            else
            {
                this.htmlDocument.LoadHtml(this.browser.Document.Body.OuterHtml);

                int remaining;

                if (this.isDocumentFullyLoaded(this.htmlDocument, out remaining))
                {
                    this.manualResetEvent.Set();
                    return;
                }

                if (completing)
                {
                    completedCount++;
                }

                if (completedCount != lastCompletedCount)
                {
                    lastCompletedCount = completedCount;
                    return;
                }

                if (this.lastRemaining == remaining)
                {
                    this.lastRemaining = 0;
                    this.browser.Navigate(this.url);
                }
                else
                {
                    this.lastRemaining = remaining;
                }
            }
        }

        private async Task<HtmlAgilityPack.HtmlDocument> Navigate()
        {
            this.manualResetEvent.Reset();
            this.browser.Navigate(this.url);

            await Task.Factory.StartNew(() =>
            {
                while(!this.manualResetEvent.WaitOne(2000))
                {
                    RefreshDocument(completing:false);
                }
            });

            return this.htmlDocument;
        }

        public static async Task<HtmlAgilityPack.HtmlDocument> Navigate(string url, IsDocumentFullyLoaded isDocumentFullyLoaded)
        {
            var request = new WebBrowserRequest(url, isDocumentFullyLoaded);
            return await request.Navigate();
        }
    }
}
