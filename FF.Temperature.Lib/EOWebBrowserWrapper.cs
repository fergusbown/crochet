using EO.WebBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public sealed class EOWebBrowserWrapper : IWebBrowser
    {
        private readonly ThreadRunner threadRunner;
        private readonly WebView webView;
        public EOWebBrowserWrapper()
        {
            threadRunner = new ThreadRunner();
            this.webView = threadRunner.CreateWebView();
            this.webView.SetOptions(new EO.WebEngine.BrowserOptions() { AllowJavaScript = true });
            this.webView.LoadCompleted += WebView_LoadCompleted;
        }

        public string Html
        {
            get
            {
                return (string)this.threadRunner.Send(() => this.webView.GetHtml() ?? String.Empty);
            }
        }

        public event EventHandler DocumentCompleted;

        public void Dispose()
        {
            this.webView.LoadCompleted -= WebView_LoadCompleted;
            this.webView.Dispose();
            this.threadRunner.Dispose();
        }

        public void Navigate(string url)
        {
            threadRunner.Post(() => webView.LoadUrl(url));
        }

        public void Stop()
        {
            this.threadRunner.Post(() => webView.StopLoad());
        }

        private void WebView_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            this.DocumentCompleted?.Invoke(this, new EventArgs());
        }
    }
}
