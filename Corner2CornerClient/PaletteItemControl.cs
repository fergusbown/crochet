using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FF.Corner2Corner.Lib;
using FF.Common;

namespace Corner2CornerClient
{
    public class PaletteItemClickEventArgs
    {
        public bool IsDoubleClick { get; }
        public IPaletteItem PaletteItem { get; }

        public PaletteItemClickEventArgs(bool isDoubleClick, IPaletteItem paletteItem)
        {
            this.IsDoubleClick = IsDoubleClick;
            this.PaletteItem = paletteItem;
        }
    }

    public partial class PaletteItemControl : UserControl, IPaletteItem
    {
        private Timer clickTimer = new Timer()
        {
            Interval = SystemInformation.DoubleClickTime,
            Enabled = false
        };

        public PaletteItemControl(
            IPaletteItem paletteItem)
        {
            InitializeComponent();
            this.label1.BackColor = paletteItem.Color;
            this.label1.Text = paletteItem.Text;
            this.label1.ForeColor = ColorHelper.ContrastingColor(paletteItem.Color);
            this.Color = paletteItem.Color;

            clickTimer.Tick += (s, e) =>
            {
                this.clickTimer.Enabled = false;
                this.PaletteItemClick?.Invoke(this, new PaletteItemClickEventArgs(false, this));
            };

            label1.DoubleClick += (s, e) =>
            {
                this.clickTimer.Enabled = false;
                this.PaletteItemDoubleClick?.Invoke(this, new PaletteItemClickEventArgs(true, this));
            };

            label1.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.clickTimer.Enabled = true;
                }
            };
        }

        public event EventHandler<PaletteItemClickEventArgs> PaletteItemClick;
        public event EventHandler<PaletteItemClickEventArgs> PaletteItemDoubleClick;


        public Color Color { get; }

        public override string Text
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }
    }
}
