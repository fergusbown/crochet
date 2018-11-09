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
        private Corner2CornerCommandOptions options;

        private TState originalState;
        private TState newState;

        protected Corner2CornerProject Project { get; }
        protected ICorner2CornerCommandsInput CommandsInput { get; }

        public bool SupportsUndo => this.options.SupportsUndo;

        public bool ClearsUndoStack => this.options.ClearsUndoStack;

        protected BaseCorner2CornerCommand(
            Corner2CornerProject project,
            ICorner2CornerCommandsInput commandsInput,
            Corner2CornerCommandOptions options)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.CommandsInput = commandsInput;
        }

        protected abstract bool CanDo(out TState currentState, out TState newState);

        protected abstract void DoImplementation(TState from, TState to);

        private void DoAndTrack(ChangeTrackingOperation operation, TState from, TState to)
        {
            DoImplementation(from, to);

            if (this.options.TracksChange)
            {
                this.Project.ChangeTracking.Track(operation);
            }

            this.CommandsInput.ProjectChange(this.options.ProjectChangeDetails);
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
