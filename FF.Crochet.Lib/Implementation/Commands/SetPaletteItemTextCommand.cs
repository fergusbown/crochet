using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SetPaletteItemText)]
    internal class SetPaletteItemTextCommand : BaseCorner2CornerCommand<IPaletteItem>, ICommand
    {
        private IPaletteItem context;

        public SetPaletteItemTextCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, new ProjectChangeDetails(paletteChanged:true))
        {
        }

        public static SetPaletteItemTextCommand CreateWithContext(
            Corner2CornerProject project, 
            ICorner2CornerCommandsInput commandsInput,
            IPaletteItem context)
        {
            SetPaletteItemTextCommand result = new SetPaletteItemTextCommand(project, commandsInput);
            result.context = context;
            return result;
        }

        protected override bool CanDo(out IPaletteItem currentState, out IPaletteItem newState)
        {
            currentState = null;
            newState = null;

            if (context == null && !this.CommandsInput.GetCurrentPaletteItem(out context))
            {
                return false;
            }

            if (this.CommandsInput.GetPaletteItemText(context, out string newText))
            {
                if (this.Project.Palette.Find(context.Color, out currentState))
                {
                    currentState = new PaletteItem(currentState);
                    newState = new PaletteItem(color: context.Color, text: newText);
                    return true;
                }
            }

            return false;
        }

        protected override void DoImplementation(IPaletteItem from, IPaletteItem to)
        {
            this.Project.Palette.TryUpdate(to.Color, to.Text, out IPaletteItem original);
        }
    }
}
