using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.AddPaletteItem)]
    internal class AddPaletteItemCommand : BaseCorner2CornerCommand<Color>, ICommand
    {
        public AddPaletteItemCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().ChangesPalette())
        {
        }

        protected override bool CanDo(out Color currentState, out Color newState)
        {
            Point clickedPoint;
            if (this.CommandsInput.GetClickImagePoint(out clickedPoint))
            {
                if (this.Project.Image != null)
                {
                    currentState = Color.Empty;
                    newState = ((Bitmap)this.Project.Image).GetPixel(clickedPoint.X, clickedPoint.Y);
                    return !this.Project.Palette.Find(newState, out IPaletteItem item);
                }
            }

            currentState = Color.Empty;
            newState = Color.Empty;
            return false;
        }

        protected override void DoImplementation(Color from, Color to)
        {
            if (to.IsEmpty)
            {
                this.Project.Palette.Remove(from);
            }
            else
            {
                this.Project.Palette.Add(to);
            }
        }
    }
}
