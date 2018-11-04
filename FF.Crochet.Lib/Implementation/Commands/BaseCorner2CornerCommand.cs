using FF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal abstract class BaseCorner2CornerCommand<TState> : ICommand
    {
        private ProjectChangeDetails projectChangeDetails;

        private TState originalState;
        private TState newState;

        protected Corner2CornerProject Project { get; }
        protected ICorner2CornerCommandsInput CommandsInput { get; }

        public bool SupportsUndo { get; }

        public bool ClearsUndoStack { get; }

        protected BaseCorner2CornerCommand(
            Corner2CornerProject project,
            ICorner2CornerCommandsInput commandsInput,
            ProjectChangeDetails projectChangeDetails,
            bool supportsUndo = true, 
            bool clearsUndoStack = false)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.projectChangeDetails = projectChangeDetails;
            this.CommandsInput = commandsInput;
            this.SupportsUndo = supportsUndo;
            this.ClearsUndoStack = clearsUndoStack;
        }

        protected abstract bool CanDo(out TState currentState, out TState newState);

        protected abstract void DoImplementation(TState from, TState to);

        public bool Do()
        {
            if (CanDo(out originalState, out newState))
            {
                this.Redo();
                return true;
            }

            return false;
        }

        public void Undo()
        {
            DoImplementation(newState, originalState);

            if (this.projectChangeDetails != null)
            {
                this.CommandsInput.ProjectChange(this.projectChangeDetails);
            }
        }

        public void Redo()
        {
            this.DoImplementation(originalState, newState);

            if (this.projectChangeDetails != null)
            {
                this.CommandsInput.ProjectChange(this.projectChangeDetails);
            }
        }
    }
}
