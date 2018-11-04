using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public interface ICommandFactory<TCommandType>
        where TCommandType : System.Enum
    {
        ICommand GetCommand(TCommandType commandType);
    }
}
