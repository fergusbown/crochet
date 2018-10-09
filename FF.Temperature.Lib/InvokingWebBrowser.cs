using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FF.Temperature.Lib
{
    public class InvokingWebBrowser : IWebBrowser
    {
        private readonly Control control;
        private readonly IWebBrowser browser;

        private T InvokeIfRequired<T>(Func<T> func)
        {
            if (this.control.InvokeRequired)
            {
                return (T)this.control.Invoke(func);
            }
            else
            {
                return func();
            }
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.control.InvokeRequired)
            {
                this.control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Browser_DocumentCompleted(object sender, EventArgs e)
        {
            this.DocumentCompleted?.Invoke(sender, e);
        }

        public InvokingWebBrowser(Control control, IWebBrowser browser)
        {
            this.control = control;
            this.browser = browser;
            this.browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        public string Html => this.InvokeIfRequired(() => this.browser.Html);

        public event EventHandler DocumentCompleted;

        public void Navigate(string url)
        {
            this.InvokeIfRequired(() => this.browser.Navigate(url));
        }

        public void Stop()
        {
            this.InvokeIfRequired(() => this.browser.Stop());
        }
    }
}
