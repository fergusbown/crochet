using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.ClickImage)]
    internal class ClickImageCommand : ICommand
    {
        ICommand command;
        private Corner2CornerProject project;
        private ICorner2CornerCommandsInput commandsInput;

        public ClickImageCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
        {
            this.project = project;
            this.commandsInput = commandsInput;
        }

        public bool SupportsUndo => true;

        public bool ClearsUndoStack => false;

        public bool Do()
        {
            if (this.project.ImageGrid != null)
            {
                this.command = new SetImageGridCellCommand(this.project, this.commandsInput);
            }
            else if (this.project.Image != null)
            {
                this.command = new AddPaletteItemCommand(this.project, this.commandsInput);
            }
            else
            {
                return false;
            }

            return this.command.Do();
        }

        public void Undo()
        {
            this.command.Undo();
        }

        public void Redo()
        {
            this.command.Redo();
        }
    }
}
