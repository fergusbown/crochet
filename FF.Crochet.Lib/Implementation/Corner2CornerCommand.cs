using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public enum Corner2CornerCommand
    {
        Load,
        Save,
        SaveAs,
        SaveIfRequired,
        Close,
        GenerateImageGrid,
        SetWidth,
        ClickImage,
        SetImageGridCell,
        AddPaletteItem,
        SetPaletteItemText,
        SetSelectedPaletteItem,
        SetGridBackground,
        SaveTextPattern,
        SaveImagePattern
    }
}
