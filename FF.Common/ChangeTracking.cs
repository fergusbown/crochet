using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public class ChangeTracking : IChangeTracking
    {
        private int changesSinceSave;

        public void Track(ChangeTrackingOperation operation)
        {
            switch (operation)
            {
                case ChangeTrackingOperation.SetNew:
                    this.changesSinceSave = 0;
                    this.IsNew = true;
                    break;
                case ChangeTrackingOperation.SetSaved:
                    this.changesSinceSave = 0;
                    this.IsNew = false;
                    break;
                case ChangeTrackingOperation.Do:
                    this.changesSinceSave = Math.Abs(this.changesSinceSave) + 1;
                    break;
                case ChangeTrackingOperation.Undo:
                    this.changesSinceSave--;
                    break;
                case ChangeTrackingOperation.Redo:
                    this.changesSinceSave++;
                    break;
                default:
                    break;
            }
        }

        public bool IsDirty => this.changesSinceSave != 0;

        public bool IsNew { get; private set; }
    }
}
