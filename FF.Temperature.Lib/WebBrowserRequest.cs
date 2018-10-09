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
    internal delegate string GetUrl(int attempt);

    internal sealed class WebBrowserRequest : IDisposable
    {
        private const int maxSilentAttempts = 2;
        private const int initialTimeout = 4000;
        private const int maxTimeout = 16000;
        private readonly GetUrl getUrl;
        private readonly IWebBrowser browser;
        private readonly IsDocumentFullyLoaded isDocumentFullyLoaded;

        private readonly IUserInteraction userInteraction;
        private ManualResetEvent manualResetEvent;

        private readonly object lockObject;

        private readonly HtmlAgilityPack.HtmlDocument htmlDocument;

        private string description;

        private WebBrowserRequest(
            IWebBrowser browser,
            GetUrl getUrl, 
            IsDocumentFullyLoaded isDocumentFullyLoaded, 
            IUserInteraction userInteraction)
        {
            this.browser = browser;
            this.browser.DocumentCompleted += Browser_DocumentCompleted;
            this.isDocumentFullyLoaded = isDocumentFullyLoaded;
            this.getUrl = getUrl;
            this.userInteraction = userInteraction;

            this.manualResetEvent = new ManualResetEvent(false);
            this.htmlDocument = new HtmlAgilityPack.HtmlDocument();
            this.lockObject = new object();
        }

        private void Browser_DocumentCompleted(object sender, EventArgs e)
        {
            this.userInteraction.Log($"Document completed for {this.description}");
            this.manualResetEvent?.Set();
        }

        private void Finish(string finishType)
        {
            this.userInteraction.Log($"{finishType} '{this.description}'");
            this.browser.Stop();
        }

        private bool RefreshDocument()
        {
            string html = this.browser.Html;
            this.htmlDocument.LoadHtml(html);

            int remaining;

            if (this.isDocumentFullyLoaded(this.htmlDocument, out remaining))
            {
                return true;
            }
            else
            {
                this.userInteraction.Log($"{remaining} readings remaining for '{this.description}'");
                return false;
            }
        }

        private async Task<HtmlAgilityPack.HtmlDocument> Navigate()
        {
            this.manualResetEvent.Reset();

            await Task.Factory.StartNew(() =>
            {
                bool tryAgain = true;
                bool finished = false;
                int attempts = 0;

                while (tryAgain && !finished)
                {
                    int timeout = initialTimeout;

                    this.description = this.getUrl(attempts);

                    this.userInteraction.Log($"Processing '{this.description}' (attempt {attempts + 1})");

                    if (attempts > 0)
                    {
                        this.browser.Navigate("www.google.com");
                    }

                    this.browser.Navigate(this.description);

                    while (!finished && timeout <= maxTimeout)
                    {
                        while (this.manualResetEvent.WaitOne(timeout))
                        {
                            this.manualResetEvent.Reset();
                        }

                        if (RefreshDocument())
                        {
                            finished = true;
                        }
                        else
                        {
                            timeout *= 2;
                        }
                    }

                    if (finished)
                    {
                        Finish("Finished");
                    }
                    else
                    {
                        Finish("Cancelled");

                        attempts++;

                        tryAgain = (attempts % maxSilentAttempts != 0)
                            || this.userInteraction.ShouldContinue($"{attempts} attempts made for {this.description}.  Try again?");
                    }
                }

            });

            return this.htmlDocument;
        }

        public static async Task<HtmlAgilityPack.HtmlDocument> Navigate(
            GetUrl getUrl, 
            IsDocumentFullyLoaded isDocumentFullyLoaded,
            IUserInteraction userInteraction,
            IWebBrowser browser)
        {
            using (var request = new WebBrowserRequest(browser, getUrl, isDocumentFullyLoaded, userInteraction))
            {
                return await request.Navigate();
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.browser.DocumentCompleted -= Browser_DocumentCompleted;
                    var mre = this.manualResetEvent;
                    this.manualResetEvent = null;
                    mre.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
