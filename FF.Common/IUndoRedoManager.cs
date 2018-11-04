using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public interface IUndoRedoManager
    {
        void Clear();

        bool Do(ICommand command);

        bool CanUndo();

        void Undo();

        bool CanRedo();

        void Redo();
    }

    public static class IUndoRedoManagerExtensions
    {
        public static void Do(this IUndoRedoManager undoRedoManager, IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                if (!undoRedoManager.Do(command))
                    return;
            }
        }
    }
}
