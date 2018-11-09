using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SetSelectedPaletteItem)]
    internal class SetSelectedPaletteItemCommand : BaseCorner2CornerCommand<IPaletteItem>, ICommand
    {
        public SetSelectedPaletteItemCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().ChangesPalette())
        {
        }

        protected override bool CanDo(out IPaletteItem currentState, out IPaletteItem newState)
        {
            if (this.CommandsInput.GetCurrentPaletteItem(out IPaletteItem paletteItem))
            {
                if (this.Project.Palette.Find(paletteItem.Color, out newState))
                {
                    currentState = this.Project.SelectedPaletteItem;
                    return true;
                }
            }

            currentState = null;
            newState = null;
            return false;
        }

        protected override void DoImplementation(IPaletteItem from, IPaletteItem to)
        {
            this.Project.SelectedPaletteItem = to;
        }
    }
}
