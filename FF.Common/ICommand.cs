using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public interface ICommand
    {
        bool Do();
        void Undo();

        void Redo();

        bool SupportsUndo { get; }

        bool ClearsUndoStack { get; }
    }
}
