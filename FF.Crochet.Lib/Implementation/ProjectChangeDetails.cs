using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public class ProjectChangeDetails
    {
        public bool ImageChanged { get; }

        public bool PaletteChanged { get; }

        public bool SelectedPaletteItemChanged { get; }

        public ProjectChangeDetails(
            bool imageChanged = false,
            bool paletteChanged = false,
            bool selectedPaletteItemChanged = false)
        {
            this.ImageChanged = imageChanged;
            this.PaletteChanged = paletteChanged;
            this.SelectedPaletteItemChanged = selectedPaletteItemChanged;
        }
    }
}
