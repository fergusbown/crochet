using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.Close)]
    internal class CloseCommand : BaseCorner2CornerCommand<string>, ICommand
    {
        private IUndoRedoManager undoRedoManager;
        public CloseCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput, IUndoRedoManager undoRedoManager)
            : base(
                  project, 
                  commandsInput, 
                  new ProjectChangeDetails(imageChanged:true, paletteChanged:true, selectedPaletteItemChanged:true),
                  supportsUndo:false,
                  clearsUndoStack:true,
                  trackChange: false)
        {
            this.undoRedoManager = undoRedoManager;
        }

        protected override bool CanDo(out string currentState, out string newState)
        {
            currentState = null;
            newState = null;

            SaveIfRequiredCommand saveCommand = new SaveIfRequiredCommand(this.Project, this.CommandsInput, this.undoRedoManager);
            return this.undoRedoManager.Do(saveCommand);
        }

        protected override void DoImplementation(string from, string to)
        {
            this.Project.Clear();
        }
    }
}
