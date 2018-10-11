using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public class CefBrowserWrapper : IWebBrowser
    {
        private readonly ChromiumWebBrowser browser;

        public CefBrowserWrapper(ChromiumWebBrowser browser)
        {
            this.browser = browser;
            this.browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            this.DocumentCompleted?.Invoke(sender, e);
        }

        public string Html
        {
            get
            {
                var frame = this.browser.GetMainFrame();

                if (frame != null)
                {
                    var asyncTask = frame?.GetSourceAsync();
                    Task.WaitAll(asyncTask);

                    return asyncTask.Result ?? String.Empty;
                }

                return String.Empty;
            }
        }

        public event EventHandler DocumentCompleted;

        public void Navigate(string url)
        {
            this.browser.Load(url);
        }

        public void Stop()
        {
            this.browser.Stop();
        }
    }
}
