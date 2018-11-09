using FF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.GenerateImageGrid)]
    internal class GenerateImageGridCommand : BaseCorner2CornerCommand<ImageGrid>, ICommand
    {
        public GenerateImageGridCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().ChangesImage())
        {
        }

        protected override bool CanDo(out ImageGrid currentState, out ImageGrid newState)
        {
            currentState = this.Project.ImageGrid;
            newState = null;

            if (this.Project.Image == null)
            {
                this.CommandsInput.ShowMessage("Load an image or project to start");
                return false;
            }

            if (this.Project.Palette.Count() < 2)
            {
                this.CommandsInput.ShowMessage("Select at least 2 colors for the pattern by clicking on the image");
                return false;
            }

            if (this.Project.Width < 10)
            {
                this.CommandsInput.ShowMessage("Width must be at least 10");
                return false;
            }

            this.CommandsInput.SetBusy(true);
            try
            {
                newState = ImageGridder.Create(this.Project.Width, this.Project.Image, this.Project.Palette);
                return true;
            }
            finally
            {
                this.CommandsInput.SetBusy(false);
            }
        }

        protected override void DoImplementation(ImageGrid from, ImageGrid to)
        {
            this.Project.ImageGrid = to;
        }
    }
}
