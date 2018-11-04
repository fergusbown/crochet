using FF.Common;
using FF.Corner2Corner.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corner2CornerClient
{
    public partial class MainForm : Form, ICorner2CornerCommandsInput
    {
        private readonly PictureBoxAdapter pictureBoxAdapter;
        private readonly OpenFileDialog openDialog = new OpenFileDialog() { Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png" };
        private readonly SaveFileDialog saveTextFileDialog = new SaveFileDialog() { DefaultExt = ".txt", Filter ="Text Files (*.txt)|*.txt" };
        private readonly SaveFileDialog saveImageFileDialog = new SaveFileDialog() { DefaultExt = ".bmp", Filter = "Bitmaps (*.bmp)|*.bmp" };
        private readonly ICorner2CornerProject project;
        private readonly IUndoRedoManager undoRedoManager;
        private IPaletteItem currentPaletteItem;
        private Point? clickImagePoint;

        public string InputWidth => this.gridWidthToolStripTextBox.Text;

        public MainForm()
        {
            InitializeComponent();

            this.pictureBoxAdapter = new PictureBoxAdapter(this.pictureBox);
            this.pictureBoxAdapter.MouseOverPixel += PictureBoxAdapter_MouseOverPixel;

            this.undoRedoManager = new UndoRedoManager();

            this.project = Corner2CornerProjectFactory.Create(this, this.undoRedoManager);

            this.undoToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Undo();
            this.redoToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Redo();

            this.openToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.Load));
            this.openToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.Load));

            this.generateGridToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.GenerateImageGrid));

            this.saveTextPatternToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveTextPattern));
            this.saveTextPatternToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveTextPattern));

            this.saveImagePatternToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveImagePattern));
            this.saveImagePatternToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveImagePattern));

            this.gridColorToolStripLabel.DoubleClick += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetGridBackground));
            this.setGridBackgroundColorToolStripMenuItem.Click += (s, e) => this.project.Commands.GetCommand(Corner2CornerCommand.SetGridBackground);

            this.pictureBoxAdapter.MouseClickPixel += (s, e) =>
            {
                this.clickImagePoint = e.Location;
                this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.ClickImage));
            };

            this.gridWidthToolStripTextBox.TextChanged += GridWidthToolStripTextBox_TextChanged;

            ProjectChange(new ProjectChangeDetails(true, true, true));

            Application.Idle += Application_Idle;
        }

        private void GridWidthToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetWidth));
        }

        private void PopulateGeneral()
        {
            this.gridWidthToolStripTextBox.TextChanged -= GridWidthToolStripTextBox_TextChanged;
            try
            {
                this.gridWidthToolStripTextBox.Text = this.project.Width.ToString();
            }
            finally
            {
                this.gridWidthToolStripTextBox.TextChanged += GridWidthToolStripTextBox_TextChanged;
            }
        }

        private void PopulateImage()
        {
            if (this.project.ImageGrid != null)
            {
                pictureBox.Image = ImageHelper.FromImageGrid(this.project.ImageGrid, this.project.GridBackgroundColor);
            }
            else
            {
                pictureBox.Image = this.project.Image;
            }
        }

        private void PopulatePalette()
        {
            this.palettePanel.Controls.Clear();

            List<PaletteItemControl> newControls = new List<PaletteItemControl>();

            foreach (IPaletteItem item in this.project.Palette)
            {
                PaletteItemControl paletteItemControl = new PaletteItemControl(item);
                paletteItemControl.PaletteItemClick += (s, e) => this.currentPaletteItem = e.PaletteItem;
                paletteItemControl.PaletteItemClick += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetSelectedPaletteItem));

                paletteItemControl.PaletteItemDoubleClick += (s, e) => this.currentPaletteItem = e.PaletteItem;
                paletteItemControl.PaletteItemDoubleClick += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetPaletteItemText));

                newControls.Add(paletteItemControl);
            }

            this.palettePanel.Controls.AddRange(newControls.ToArray());
        }

        private void PopulateSelectedPaletteItem()
        {
            this.activeColorToolStripLabel.BackColor = this.project.SelectedPaletteItem?.Color ?? SystemColors.Window;
        }


        private void Application_Idle(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = this.undoRedoManager.CanUndo();
            this.redoToolStripMenuItem.Enabled = this.undoRedoManager.CanRedo();
            this.gridColorToolStripLabel.BackColor = this.project.GridBackgroundColor;
        }

        //private void Commands_SetHint(object sender, MessageEventArgs e)
        //{
        //    this.hintLabel.Text = e.Message;
        //}

        private void PictureBoxAdapter_MouseOverPixel(object sender, MousePixelEventsArgs e)
        {
            if (this.project.ImageGrid == null)
            {
                this.activeColorToolStripLabel.BackColor = e.Color;
            }
        }

        public void SetBusy(bool isBusy)
        {
            if (isBusy)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void ProjectChange(ProjectChangeDetails details)
        {
            PopulateGeneral();

            if (details.ImageChanged)
            {
                PopulateImage();
            }

            if (details.PaletteChanged)
            {
                PopulatePalette();
            }

            if (details.SelectedPaletteItemChanged)
            {
                PopulateSelectedPaletteItem();
            }
        }

        public bool SelectProjectFile(out string inputFile)
        {
            if (openDialog.ShowDialog(this) == DialogResult.OK)
            {
                inputFile = openDialog.FileName;
                return true;
            }

            inputFile = null;
            return false;
        }

        public bool GetClickImagePoint(out Point point)
        {
            if (this.clickImagePoint.HasValue)
            {
                point = this.clickImagePoint.Value;
                return true;
            }
            else
            {
                point = Point.Empty;
                return false;
            }
        }

        public bool GetCurrentPaletteItem(out IPaletteItem paletteItem)
        {
            paletteItem = this.currentPaletteItem;
            return paletteItem != null;
        }

        public bool GetPaletteItemText(IPaletteItem paletteItem, out string newText)
        {
            newText = ColorNameForm.Show(this, paletteItem);
            return !string.IsNullOrWhiteSpace(newText);
        }

        public bool GetColor(out Color color)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    color = dialog.Color;
                    return true;
                }
            }

            color = Color.Empty;
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public bool SelectTextPatternFile(out string outputFile)
        {
            if (saveTextFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                outputFile = saveTextFileDialog.FileName;
                return true;
            }

            outputFile = null;
            return false;
        }

        public bool SelectImagePatternFile(out string outputFile)
        {
            if (saveImageFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                outputFile = saveImageFileDialog.FileName;
                return true;
            }

            outputFile = null;
            return false;
        }
    }
}
