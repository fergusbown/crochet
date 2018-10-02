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

        public WebBrowserWrapper()
        {
            browser = new WebBrowser();
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
                if (this.browser.InvokeRequired)
                {
                    Func<string> getHtml = () => this.Html;
                    return (string)this.browser.Invoke(getHtml);
                }
                else
                {
                    return this.browser.Document?.Body?.OuterHtml ?? String.Empty;
                }
            }
        }

        public event EventHandler DocumentCompleted;

        public void Dispose()
        {
            this.browser.DocumentCompleted -= Browser_DocumentCompleted;
            this.browser.Dispose();
        }

        public void Navigate(string url)
        {
            if (this.browser.InvokeRequired)
            {
                Action invoke = () => this.Navigate(url);
                this.browser.Invoke(invoke);
            }
            else
            {
                this.browser.Navigate(url);
            }
        }

        public void Stop()
        {
            if (this.browser.InvokeRequired)
            {
                Action invoke = () => this.Stop();
                this.browser.Invoke(invoke);
            }
            else
            {
                this.browser.Stop();
            }
        }
    }
}
