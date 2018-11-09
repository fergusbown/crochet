using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SaveIfRequired)]
    internal class SaveIfRequiredCommand : ICommand
    {
        private SaveCommand saveCommand;
        private Corner2CornerProject project;
        private ICorner2CornerCommandsInput commandsInput;
        private IUndoRedoManager undoRedoManager;
        public SaveIfRequiredCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput, IUndoRedoManager undoRedoManager)
        {
            this.saveCommand = new SaveCommand(project, commandsInput);
            this.project = project;
            this.commandsInput = commandsInput;
            this.undoRedoManager = undoRedoManager;
        }

        public bool SupportsUndo => this.saveCommand.SupportsUndo;

        public bool ClearsUndoStack => this.saveCommand.ClearsUndoStack;

        public bool Do()
        {
            if (this.project.ChangeTracking.IsDirty)
            {
                bool save = this.commandsInput.Confirm("Do you want to save changes to the current project", out bool cancelled);

                if (cancelled)
                {
                    return false;
                }
                else if (save)
                {
                    return this.undoRedoManager.Do(saveCommand);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public void Redo()
        {
            this.saveCommand.Redo();
        }

        public void Undo()
        {
            this.saveCommand.Undo();
        }
    }
}
