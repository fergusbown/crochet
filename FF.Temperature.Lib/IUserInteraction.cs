using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public interface IUserInteraction
    {
        bool ShouldContinue(string message);

        void Log(string message);
    }
}
