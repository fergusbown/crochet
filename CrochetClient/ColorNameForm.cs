using FF.Crochet.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrochetClient
{
    public partial class ColorNameForm : Form
    {
        public ColorNameForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        public static bool Show(IPaletteItem paletteItem)
        {
            using (var form = new ColorNameForm())
            {
                form.BackColor = paletteItem.Color;
                form.textBox1.Text = paletteItem.Text;

                if (form.ShowDialog(paletteItem as IWin32Window) == DialogResult.OK)
                {
                    paletteItem.Text = form.textBox1.Text.Trim();
                    return true;
                }
            }

            return false;
        }
    }
}
