using FF.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SaveTextPattern)]
    internal class SaveTextPatternCommand : BaseCorner2CornerCommand<string>, ICommand
    {
        private readonly IUndoRedoManager undoRedoManager;

        public SaveTextPatternCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput, IUndoRedoManager undoRedoManager)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().WithoutUndo())
        {
            this.undoRedoManager = undoRedoManager;
        }

        protected override bool CanDo(out string currentState, out string newState)
        {
            currentState = null;
            newState = null;
            if (this.Project.ImageGrid == null)
            {
                this.CommandsInput.ShowMessage("Please load and process image first");
                return false;
            }

            foreach (IPaletteItem item in this.Project.Palette.ToList())
            {
                if (String.IsNullOrWhiteSpace(item.Text))
                {
                    if (!this.undoRedoManager.Do(SetPaletteItemTextCommand.CreateWithContext(this.Project, this.CommandsInput, item)))
                    {
                        return false;
                    }
                }
            }

            if (this.Project.Palette.Select(p => p.Text).Distinct().Count() != this.Project.Palette.Count())
            {
                this.CommandsInput.ShowMessage("Please set a unique name for each selected color by double clicking it");
                return false;
            }

            return this.CommandsInput.SelectTextPatternFile(out newState);
        }

        protected override void DoImplementation(string from, string to)
        {
            this.CommandsInput.SetBusy(true);
            try
            {
                File.WriteAllText(to, this.Project.ImageGrid.GenerateTextPattern(this.CommandsInput.TextPatternStart));
            }
            finally
            {
                this.CommandsInput.SetBusy(false);
            }
        }
    }
}
