using FF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal class Corner2CornerCommandOptions
    {
        public ProjectChangeDetails ProjectChangeDetails { get; private set; }

        public bool SupportsUndo { get; private set; } = true;

        public bool ClearsUndoStack { get; private set; } = false;

        public bool TracksChange { get; private set; } = true;

        public static Corner2CornerCommandOptions New()
        {
            Corner2CornerCommandOptions result = new Corner2CornerCommandOptions()
            {
                ProjectChangeDetails = new ProjectChangeDetails()
            };

            return result;
        }

        public Corner2CornerCommandOptions ChangesEverything()
        {
            this.ProjectChangeDetails.ImageChanged = true;
            this.ProjectChangeDetails.PaletteChanged = true;
            this.ProjectChangeDetails.SelectedPaletteItemChanged = true;

            return this;
        }

        public Corner2CornerCommandOptions ChangesImage()
        {
            this.ProjectChangeDetails.ImageChanged = true;
            this.ProjectChangeDetails.SelectedPaletteItemChanged = true;

            return this;
        }

        public Corner2CornerCommandOptions ChangesPalette()
        {
            this.ProjectChangeDetails.PaletteChanged = true;
            this.ProjectChangeDetails.SelectedPaletteItemChanged = true;

            return this;
        }

        public Corner2CornerCommandOptions ResetsOperations()
        {
            this.SupportsUndo = false;
            this.ClearsUndoStack = true;
            this.TracksChange = false;

            return this;
        }

        public Corner2CornerCommandOptions WithoutUndo()
        {
            this.SupportsUndo = false;
            this.TracksChange = false;

            return this;
        }
    }
}
