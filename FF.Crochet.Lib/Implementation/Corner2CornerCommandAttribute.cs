using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Corner2CornerCommandAttribute : Attribute
    {
        public Corner2CornerCommand Command { get; }
        public Corner2CornerCommandAttribute(Corner2CornerCommand command)
        {
            this.Command = command;
        }
    }
}
