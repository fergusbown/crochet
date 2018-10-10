using EO.WebBrowser;
using EO.WinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public sealed class EOWebBrowserWrapper : IWebBrowser
    {
        private readonly WebControl webControl;
        private readonly WebView webView;

        public EOWebBrowserWrapper(WebControl webControl, WebView webView)
        {
            this.webControl = webControl;
            this.webView = webView;
            this.webView.SetOptions(new EO.WebEngine.BrowserOptions() { AllowJavaScript = true });
            this.webView.LoadCompleted += WebView_LoadCompleted;
            this.webView.CertificateError += WebView_CertificateError;
        }

        private void WebView_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            e.Continue();
        }

        public string Html
        {
            get
            {
                return this.webView.GetHtml() ?? String.Empty;
            }
        }

        public event EventHandler DocumentCompleted;

        public void Navigate(string url)
        {
            this.webView.LoadUrlAndWait(url);
        }

        public void Stop()
        {
            this.webView.StopLoad();
        }

        private void WebView_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            this.DocumentCompleted?.Invoke(this, new EventArgs());
        }
    }
}
