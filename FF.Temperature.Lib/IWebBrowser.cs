using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public interface IWebBrowser : IDisposable
    {
        void Navigate(string url);

        string Html { get; }

        event EventHandler DocumentCompleted;

        void Stop();
    }
}
