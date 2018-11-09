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
        private bool trackChange;

        protected Corner2CornerProject Project { get; }
        protected ICorner2CornerCommandsInput CommandsInput { get; }

        public bool SupportsUndo { get; }

        public bool ClearsUndoStack { get; }

        protected BaseCorner2CornerCommand(
            Corner2CornerProject project,
            ICorner2CornerCommandsInput commandsInput,
            ProjectChangeDetails projectChangeDetails,
            bool supportsUndo = true,
            bool clearsUndoStack = false,
            bool trackChange = true)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.projectChangeDetails = projectChangeDetails;
            this.CommandsInput = commandsInput;
            this.SupportsUndo = supportsUndo;
            this.ClearsUndoStack = clearsUndoStack;
            this.trackChange = trackChange;
        }

        protected abstract bool CanDo(out TState currentState, out TState newState);

        protected abstract void DoImplementation(TState from, TState to);

        private void DoAndTrack(ChangeTrackingOperation operation, TState from, TState to)
        {
            DoImplementation(from, to);

            if (this.trackChange)
            {
                this.Project.ChangeTracking.Track(operation);
            }

            if (this.projectChangeDetails != null)
            {
                this.CommandsInput.ProjectChange(this.projectChangeDetails);
            }
        }

        public bool Do()
        {
            if (CanDo(out originalState, out newState))
            {
                DoAndTrack(ChangeTrackingOperation.Do, originalState, newState);
                return true;
            }

            return false;
        }

        public void Undo()
        {
            DoAndTrack(ChangeTrackingOperation.Undo, newState, originalState);
        }

        public void Redo()
        {
            DoAndTrack(ChangeTrackingOperation.Redo, originalState, newState);
        }
    }
}
