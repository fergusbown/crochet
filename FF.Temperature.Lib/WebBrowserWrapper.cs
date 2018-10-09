using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FF.Temperature.Lib
{
    public sealed class WebBrowserWrapper : IWebBrowser
    {
        private readonly WebBrowser browser;

        public WebBrowserWrapper(WebBrowser browser)
        {
            this.browser = browser;
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.DocumentCompleted?.Invoke(this, new EventArgs());
        }

        public string Html
        {
            get
            {
                return this.browser.Document?.Body?.OuterHtml ?? String.Empty;
            }
        }

        public event EventHandler DocumentCompleted;

        public void Navigate(string url)
        {
            this.browser.Navigate(url);
        }

        public void Stop()
        {
            this.browser.Stop();
        }
    }
}
