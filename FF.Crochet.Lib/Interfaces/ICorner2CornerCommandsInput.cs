using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public interface ICorner2CornerCommandsInput : IShowMessage
    {
        string InputWidth { get; }

        void SetBusy(bool isBusy);

        void ProjectChange(ProjectChangeDetails details);

        bool SelectLoadProjectFile(out string inputFile);

        bool SelectSaveProjectFile(out string outputFile);

        bool GetClickImagePoint(out Point point);

        bool GetCurrentPaletteItem(out IPaletteItem paletteItem);

        bool GetPaletteItemText(IPaletteItem paletteItem, out string newText);

        bool GetColor(out Color color);

        bool SelectTextPatternFile(out string outputFile);

        bool SelectImagePatternFile(out string outputFile);

        TextPatternStart TextPatternStart { get; }
    }
}
