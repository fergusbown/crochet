using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal class LoadCommandState
    {
        private Image image;
        private Corner2CornerPalette palette;
        private ImageGrid grid;
        private IPaletteItem selected;

        public LoadCommandState(Corner2CornerProject project)
        {
            this.image = project.Image;
            this.palette = project.Palette;
            this.grid = project.ImageGrid;
            this.selected = project.SelectedPaletteItem;
        }

        public LoadCommandState(Image image)
        {
            this.image = image;
            this.palette = new Corner2CornerPalette();
        }

        public void ApplyTo(Corner2CornerProject project)
        {
            project.Image = image;
            project.Palette = palette;
            project.ImageGrid = grid;
            project.SelectedPaletteItem = selected;
        }
    }

    [Corner2CornerCommand(Corner2CornerCommand.Load)]
    internal class LoadCommand : BaseCorner2CornerCommand<LoadCommandState>, ICommand
    {
        public LoadCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, new ProjectChangeDetails(imageChanged:true, paletteChanged:true, selectedPaletteItemChanged:true))
        {
        }

        protected override bool CanDo(out LoadCommandState currentState, out LoadCommandState newState)
        {
            currentState = new LoadCommandState(this.Project);

            if (this.CommandsInput.SelectProjectFile(out string fileName))
            {
                this.CommandsInput.SetBusy(true);
                try
                {
                    newState = new LoadCommandState(Image.FromFile(fileName));
                    return true;
                }
                finally
                {
                    this.CommandsInput.SetBusy(false);
                }
            }

            newState = null;
            return false;
        }

        protected override void DoImplementation(LoadCommandState from, LoadCommandState to)
        {
            to.ApplyTo(this.Project);
        }
    }
}
