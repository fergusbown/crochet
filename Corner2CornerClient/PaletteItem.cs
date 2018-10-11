using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FF.Crochet.Lib;

namespace Corner2CornerClient
{
    public partial class PaletteItem : UserControl, IPaletteItem
    {
        public PaletteItem(Color color)
        {
            InitializeComponent();
            this.label1.BackColor = color;
            this.label1.ForeColor = ColorHelper.ContrastingColor(color);
            this.Color = color;
        }

        public Color Color { get; }

        public event EventHandler<IPaletteItem> ColorClick;

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


        private void label1_DoubleClick(object sender, EventArgs e)
        {
            ColorNameForm.Show(this);
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.ColorClick?.Invoke(this, this);
            }
        }
    }
}
