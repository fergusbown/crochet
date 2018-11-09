using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SetGridBackground)]
    internal class SetGridBackgroundCommand : BaseCorner2CornerCommand<Color>, ICommand
    {
        public SetGridBackgroundCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, Corner2CornerCommandOptions.New().ChangesImage())
        {
        }

        protected override bool CanDo(out Color currentState, out Color newState)
        {
            currentState = this.Project.GridBackgroundColor;

            if (this.CommandsInput.GetColor(out Color color))
            {
                newState = color;
                return true;
            }

            newState = Color.Empty;
            return false;
        }

        protected override void DoImplementation(Color from, Color to)
        {
            this.Project.GridBackgroundColor = to;
        }
    }
}
