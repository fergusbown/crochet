﻿using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    [Corner2CornerCommand(Corner2CornerCommand.SaveAs)]
    internal class SaveAsCommand : BaseCorner2CornerCommand<string>, ICommand
    {
        public SaveAsCommand(Corner2CornerProject project, ICorner2CornerCommandsInput commandsInput)
            : base(
                  project, 
                  commandsInput, 
                  new ProjectChangeDetails(),
                  supportsUndo:false,
                  clearsUndoStack:false,
                  trackChange:false)
        {
        }

        protected override bool CanDo(out string currentState, out string newState)
        {
            currentState = null;
            return this.CommandsInput.SelectSaveProjectFile(out newState);
        }

        protected override void DoImplementation(string from, string to)
        {
            try
            {
                this.CommandsInput.SetBusy(true);
                try
                {
                    Corner2CornerProjectPersistence.Save(to, this.Project);
                }
                finally
                {
                    this.CommandsInput.SetBusy(false);
                }
            }
            catch (Exception e)
            {
                this.CommandsInput.ShowMessage($"Could not save file: {e.Message}");
            }
        }
    }
}
