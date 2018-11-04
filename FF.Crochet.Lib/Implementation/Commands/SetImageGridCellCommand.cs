using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SetImageGridCell)]
    internal class SetImageGridCellCommand : BaseCorner2CornerCommand<IPaletteItem>, ICommand
    {
        private Point clickedPoint;

        public SetImageGridCellCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, new ProjectChangeDetails(imageChanged:true))
        {
        }

        protected override bool CanDo(out IPaletteItem currentState, out IPaletteItem newState)
        {
            currentState = null;
            newState = null;

            if (this.CommandsInput.GetClickImagePoint(out clickedPoint))
            {
                clickedPoint = ImageHelper.ToImageGridPoint(clickedPoint);

                if (this.Project.ImageGrid != null)
                {
                    if (this.Project.SelectedPaletteItem == null)
                    {
                        this.CommandsInput.ShowMessage("Select a color from the palette before trying to edit the pattern");
                        return false;
                    }

                    currentState = this.Project.ImageGrid[clickedPoint.X, clickedPoint.Y];
                    newState = this.Project.SelectedPaletteItem;
                    return true;
                }
            }

            return false;
        }

        protected override void DoImplementation(IPaletteItem from, IPaletteItem to)
        {
            this.Project.ImageGrid[clickedPoint.X, clickedPoint.Y] = to;
        }
    }
}
