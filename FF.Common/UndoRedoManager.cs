using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public class UndoRedoManager : IUndoRedoManager
    {
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();

        public bool CanRedo()
        {
            return this.redoStack.Count > 0;
        }

        public bool CanUndo()
        {
            return this.undoStack.Count > 0;
        }

        public void Clear()
        {
            this.redoStack.Clear();
            this.undoStack.Clear();
        }

        public bool Do(ICommand command)
        {
            if (command.Do())
            {
                this.redoStack.Clear();

                if (command.SupportsUndo)
                {
                    this.undoStack.Push(command);
                }

                if (command.ClearsUndoStack)
                {
                    this.undoStack.Clear();
                }

                return true;
            }

            return false;
        }

        public void Redo()
        {
            if (CanRedo())
            {
                var redoCommand = this.redoStack.Pop();
                redoCommand.Redo();
                this.undoStack.Push(redoCommand);
            }
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var undoCommand = this.undoStack.Pop();
                undoCommand.Undo();
                this.redoStack.Push(undoCommand);
            }
        }
    }
}
