using FF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SetWidth)]
    internal class SetWidthCommand : BaseCorner2CornerCommand<int>, ICommand
    {
        public SetWidthCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(project, commandsInput, new ProjectChangeDetails())
        {
        }

        protected override bool CanDo(out int currentState, out int newState)
        {
            currentState = this.Project.Width;

            if (!string.IsNullOrEmpty(this.CommandsInput.InputWidth))
            {
                if (int.TryParse(this.CommandsInput.InputWidth, out newState))
                {
                    return true;
                }
            }

            newState = currentState;
            return false;
        }

        protected override void DoImplementation(int from, int to)
        {
            this.Project.Width = to;
        }
    }
}
