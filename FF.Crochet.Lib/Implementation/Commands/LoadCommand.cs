using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.Load)]
    internal class LoadCommand : BaseCorner2CornerCommand<string>, ICommand
    {
        private IUndoRedoManager undoRedoManager;
        public LoadCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput, IUndoRedoManager undoRedoManager)
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

            return this.undoRedoManager.Do(saveCommand) && this.CommandsInput.SelectLoadProjectFile(out newState);
        }

        protected override void DoImplementation(string from, string to)
        {
            try
            {
                this.CommandsInput.SetBusy(true);
                try
                {
                    if (!Corner2CornerProjectPersistence.Load(to, this.Project))
                    {
                        this.Project.Clear();

                        var bytes = File.ReadAllBytes(to);
                        this.Project.Image = Image.FromStream(new MemoryStream(bytes));
                    }
                }
                finally
                {
                    this.CommandsInput.SetBusy(false);
                }
            }
            catch
            {
                this.CommandsInput.ShowMessage($"Could not load file {to}");
            }
        }
    }
}
