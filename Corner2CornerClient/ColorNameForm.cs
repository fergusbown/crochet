using FF.Corner2Corner.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corner2CornerClient
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

        public static string Show(IWin32Window owner, IPaletteItem paletteItem)
        {
            using (var form = new ColorNameForm())
            {
                form.BackColor = paletteItem.Color;
                form.textBox1.Text = paletteItem.Text;

                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    return form.textBox1.Text.Trim();
                }
            }

            return null;
        }
    }
}
