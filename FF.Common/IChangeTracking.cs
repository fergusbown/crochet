using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public enum ChangeTrackingOperation
    {
        SetNew,
        SetSaved,
        Do,
        Undo,
        Redo
    }

    public interface IChangeTracking
    {
        void Track(ChangeTrackingOperation operation);

        bool IsDirty { get; }

        bool IsNew { get; }
    }
}
