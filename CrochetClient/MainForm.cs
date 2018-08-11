using FF.Crochet.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrochetClient
{
    public partial class MainForm : Form
    {
        private readonly PictureBoxAdapter pictureBoxAdapter;
        private readonly OpenFileDialog openDialog = new OpenFileDialog() { Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png" }; 
        private readonly SaveFileDialog saveFileDialog = new SaveFileDialog() { DefaultExt = ".txt", Filter ="Text Files (*.txt)|*.txt" };
        private readonly SaveFileDialog saveImageFileDialog = new SaveFileDialog() { DefaultExt = ".bmp", Filter = "Bitmaps (*.bmp)|*.bmp" };
        private ImageGrid imageGrid;

        private void SetHint()
        {
            string hint;
            if (this.imageGrid == null)
                hint = "Process to generate pattern";
            else
                hint = $"Pattern dimensions: {this.imageGrid.Width} x {this.imageGrid.Height}.  Select color and click on image to modify";

            hintLabel.Text = hint;
        }

        public MainForm()
        {
            InitializeComponent();
            this.pictureBoxAdapter = new PictureBoxAdapter(this.pictureBox);
            this.pictureBoxAdapter.MouseOverPixel += PictureBoxAdapter_MouseOverPixel;
            this.pictureBoxAdapter.MouseClickPixel += PictureBoxAdapter_MouseClickPixel;
        }

        private void PictureBoxAdapter_MouseClickPixel(object sender, MousePixelEventsArgs e)
        {
            if (this.imageGrid == null)
            {
                var paletteItem = new PaletteItem(e.Color) { Height = 20 };
                paletteItem.ColorClick += PaletteItem_ColorClick;
                this.palettePanel.Controls.Add(paletteItem);
            }
            else
            {
                var selectedPaletteItem = this.Palette
                    .Where(p => p.Color == currentColorPanel.BackColor)
                    .FirstOrDefault();

                if (selectedPaletteItem == null)
                {
                    this.ShowMessage("Please select a palette item to use by clicking on it");
                    return;
                }

                Point gridPoint = ImageHelper.ToImageGridPoint(e.Location);
                this.imageGrid[gridPoint.X, gridPoint.Y] = selectedPaletteItem.Color;
                pictureBox.Image = ImageHelper.FromImageGrid(imageGrid, panelGridColor.BackColor);
            }
        }

        private void PaletteItem_ColorClick(object sender, IPaletteItem e)
        {
            if (this.imageGrid != null)
            {
                this.currentColorPanel.BackColor = e.Color;
            }
        }

        private void PictureBoxAdapter_MouseOverPixel(object sender, MousePixelEventsArgs e)
        {
            if (this.imageGrid == null)
            {
                this.currentColorPanel.BackColor = e.Color;
            }
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    pictureBox.Load(openDialog.FileName);
                    this.palettePanel.Controls.Clear();
                    this.imageGrid = null;
                    SetHint();
                }
                catch
                {
                    openDialog.FileName = null;
                }
            }
        }

        private void ShowMessage(string text, MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            MessageBox.Show(this, text, this.Text, MessageBoxButtons.OK, icon);
        }

        private IEnumerable<IPaletteItem> Palette
        {
            get
            {
                return this.palettePanel.Controls.Cast<PaletteItem>();
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.openDialog.FileName))
            {
                ShowMessage("Please select an image to process");
                return;
            }

            if (this.Palette.Count() < 2)
            {
                ShowMessage("Please select at least 2 colors for the pattern by clicking on the image");
                return;
            }

            int width;
            if (!int.TryParse(gridWidthTextBox.Text, out width))
            {
                ShowMessage("Please specify a valid width for the pattern");
            }

            if (width < 10)
            {
                ShowMessage("Width must be at least 10");
            }

            ImageGridder gridder = new ImageGridder(width);
            using (Stream image = File.OpenRead(openDialog.FileName))
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.imageGrid = gridder.Load(image, this.Palette.Select(p => p.Color));
                    pictureBox.Image = ImageHelper.FromImageGrid(imageGrid, panelGridColor.BackColor);
                    this.currentColorPanel.BackColor = SystemColors.Control;
                    SetHint();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void panelGridColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = panelGridColor.BackColor;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    panelGridColor.BackColor = dialog.Color;
                }
            }
        }

        private void buttonTextPattern_Click(object sender, EventArgs e)
        {
            if (this.imageGrid == null)
            {
                this.ShowMessage("Please load and process image first");
                return;
            }

            foreach (IPaletteItem item in this.Palette)
            {
                if (String.IsNullOrEmpty(item.Text))
                {
                    if (!ColorNameForm.Show(item))
                    {
                        return;
                    }
                }
            }

            if (this.Palette.Select(p => p.Text).Distinct().Count() != this.Palette.Count())
            {
                this.ShowMessage("Please set a unique name for each selected color by double clicking it");
                return;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, this.imageGrid.GenerateTextPattern(this.Palette));
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void buttonImagePattern_Click(object sender, EventArgs e)
        {
            if (this.imageGrid == null)
            {
                this.ShowMessage("Please load and process image first");
                return;
            }

            if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    using (var pattern = ImageHelper.FromImageGrid(this.imageGrid, panelGridColor.BackColor, true))
                    {
                        pattern.Save(saveImageFileDialog.FileName);
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
    }
}
