﻿using FF.Common;
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
        private readonly OpenFileDialog openDialog = new OpenFileDialog() { Filter = "Image or Corner 2 Corner project Files (*.bmp, *.jpg, *.png, *.c2c)|*.bmp;*.jpg;*.png;*.c2c" };
        private readonly SaveFileDialog saveTextFileDialog = new SaveFileDialog() { DefaultExt = ".txt", Filter ="Text Files (*.txt)|*.txt" };
        private readonly SaveFileDialog saveImageFileDialog = new SaveFileDialog() { DefaultExt = ".bmp", Filter = "Bitmaps (*.bmp)|*.bmp" };
        private readonly SaveFileDialog saveProjectFileDialog = new SaveFileDialog() { DefaultExt = ".c2c", Filter = "Corner 2 Corner projects (*.c2c)|*.c2c" };
        private readonly ICorner2CornerProject project;
        private readonly IUndoRedoManager undoRedoManager;
        private IPaletteItem currentPaletteItem;
        private Point? clickImagePoint;

        public string InputWidth => this.gridWidthToolStripTextBox.Text;

        public TextPatternStart TextPatternStart => Properties.Settings.Default.TextPatternStart;

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

            this.saveToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.Save));
            this.saveToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.Save));

            this.saveAsToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveAs));

            this.closeProjectToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.Close));

            this.generateGridToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.GenerateImageGrid));
            this.generateGridToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.GenerateImageGrid));

            this.saveTextPatternToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveTextPattern));
            this.saveTextPatternToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveTextPattern));

            this.saveImagePatternToolStripButton.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveImagePattern));
            this.saveImagePatternToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveImagePattern));

            this.gridColorToolStripLabel.DoubleClick += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetGridBackground));
            this.setGridBackgroundColorToolStripMenuItem.Click += (s, e) => this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SetGridBackground));

            this.pictureBoxAdapter.MouseClickPixel += (s, e) =>
            {
                this.clickImagePoint = e.Location;
                this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.ClickImage));
            };

            this.FormClosing += (s, e) =>
            {
                e.Cancel = !this.undoRedoManager.Do(this.project.Commands.GetCommand(Corner2CornerCommand.SaveIfRequired));
            };

            this.exitToolStripMenuItem.Click += (s, e) => this.Close();

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
                paletteItemControl.Width = this.palettePanel.ClientRectangle.Width - 4;
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

        private const int maxCaptionLength = 80;

        private string GetCaption()
        {
            string description = this.project.FileName ?? "(New Project)";

            if (description.Length > maxCaptionLength)
            {
                description = "..." + description.Substring(description.Length - maxCaptionLength);
            }

            if (this.project.ChangeTracking.IsDirty || this.project.ChangeTracking.IsNew)
                description = description + "*";

            description = description + " - Cornifier";

            return description;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = this.undoRedoManager.CanUndo();
            this.redoToolStripMenuItem.Enabled = this.undoRedoManager.CanRedo();
            this.gridColorToolStripLabel.BackColor = this.project.GridBackgroundColor;
            this.toolStripStatusLabel1.Text = this.project.ImageGrid == null ? "" : $"Current grid dimensions: {this.project.ImageGrid.Width} x {this.project.ImageGrid.Height}";
            this.topLeftToolStripMenuItem.Checked = Properties.Settings.Default.TextPatternStart == TextPatternStart.TopLeft;
            this.bottomRightToolStripMenuItem.Checked = Properties.Settings.Default.TextPatternStart == TextPatternStart.BottomRight;
            this.Text = GetCaption();
        }

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

        public bool SelectLoadProjectFile(out string inputFile)
        {
            return ShowFileDialog(openDialog, out inputFile);
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
            MessageBox.Show(this, message, "Cornifier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public bool Confirm(string message, out bool cancelled)
        {
            DialogResult result = MessageBox.Show(this, message, "Cornifier", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            switch (result)
            {
                case DialogResult.Yes:
                    cancelled = false;
                    return true;
                case DialogResult.No:
                    cancelled = false;
                    return false;
                default:
                    cancelled = true;
                    return false;
            }
        }


        private bool ShowFileDialog(FileDialog dialog, out string file)
        {
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                file = dialog.FileName;
                return true;
            }

            file = null;
            return false;
        }

        public bool SelectTextPatternFile(out string outputFile)
        {
            return ShowFileDialog(saveTextFileDialog, out outputFile);
        }

        public bool SelectImagePatternFile(out string outputFile)
        {
            return ShowFileDialog(saveImageFileDialog, out outputFile);
        }

        public bool SelectSaveProjectFile(out string outputFile)
        {
            return ShowFileDialog(saveProjectFileDialog, out outputFile);
        }

        private void topLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TextPatternStart = TextPatternStart.TopLeft;
            Properties.Settings.Default.Save();
        }

        private void bottomRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TextPatternStart = TextPatternStart.BottomRight;
            Properties.Settings.Default.Save();
        }
    }
}
