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

    internal sealed class WebBrowserRequest : IDisposable
    {
        private const int maxSilentAttempts = 2;
        private const int maxTimeout = 32000;

        private readonly IWebBrowser browser;
        private readonly IsDocumentFullyLoaded isDocumentFullyLoaded;
        private readonly string url;
        private readonly IUserInteraction userInteraction;
        private readonly ManualResetEvent manualResetEvent;

        private readonly object lockObject;

        private readonly HtmlAgilityPack.HtmlDocument htmlDocument;

        private string description => this.url;

        private WebBrowserRequest(
            IWebBrowser browser,
            string url, 
            IsDocumentFullyLoaded isDocumentFullyLoaded, 
            IUserInteraction userInteraction)
        {
            this.browser = browser;
            this.browser.DocumentCompleted += Browser_DocumentCompleted;
            this.isDocumentFullyLoaded = isDocumentFullyLoaded;
            this.url = url;
            this.userInteraction = userInteraction;

            this.manualResetEvent = new ManualResetEvent(false);
            this.htmlDocument = new HtmlAgilityPack.HtmlDocument();
            this.lockObject = new object();
        }

        private void Browser_DocumentCompleted(object sender, EventArgs e)
        {
            this.userInteraction.Log($"Document completed for {this.description}");
            this.manualResetEvent.Set();
        }

        private void Finish(string finishType)
        {
            this.userInteraction.Log($"{finishType} '{this.description}'");
            this.browser.Stop();
        }

        private bool RefreshDocument()
        {
            this.htmlDocument.LoadHtml(this.browser.Html);

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
                    int timeout = 2000;

                    this.userInteraction.Log($"Processing '{this.description}' (attempt {attempts + 1})");
                    this.browser.Navigate(this.url);

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
            string url, 
            IsDocumentFullyLoaded isDocumentFullyLoaded,
            IUserInteraction userInteraction)
        {
            var browser = new WebBrowserWrapper();
            //var browser = new EOWebBrowserWrapper();

            using (var request = new WebBrowserRequest(browser, url, isDocumentFullyLoaded, userInteraction))
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
                    this.browser.Dispose();
                    this.manualResetEvent.Dispose();
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
