using FF.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SaveImagePattern)]
    internal class SaveImagePatternCommand : BaseCorner2CornerCommand<string>, ICommand
    {
        public SaveImagePatternCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().WithoutUndo())
        {
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

            return this.CommandsInput.SelectImagePatternFile(out newState);
        }

        protected override void DoImplementation(string from, string to)
        {
            this.CommandsInput.SetBusy(true);
            try
            {
                using (var pattern = ImageHelper.FromImageGrid(this.Project.ImageGrid, this.Project.GridBackgroundColor, true))
                {
                    pattern.Save(to);
                }
            }
            finally
            {
                this.CommandsInput.SetBusy(false);
            }
        }
    }
}
