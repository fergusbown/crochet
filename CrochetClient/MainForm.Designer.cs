namespace CrochetClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadImageButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.currentColorPanel = new System.Windows.Forms.Panel();
            this.processButton = new System.Windows.Forms.Button();
            this.palettePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.gridWidthTextBox = new System.Windows.Forms.MaskedTextBox();
            this.labelGridWidth = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelGridColor = new System.Windows.Forms.Panel();
            this.buttonTextPattern = new System.Windows.Forms.Button();
            this.buttonImagePattern = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.hintLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadImageButton
            // 
            this.loadImageButton.Location = new System.Drawing.Point(12, 12);
            this.loadImageButton.Name = "loadImageButton";
            this.loadImageButton.Size = new System.Drawing.Size(108, 23);
            this.loadImageButton.TabIndex = 0;
            this.loadImageButton.Text = "Load Image";
            this.loadImageButton.UseVisualStyleBackColor = true;
            this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.Color.Aqua;
            this.pictureBox.Location = new System.Drawing.Point(12, 41);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(715, 381);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // currentColorPanel
            // 
            this.currentColorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentColorPanel.Location = new System.Drawing.Point(734, 41);
            this.currentColorPanel.Name = "currentColorPanel";
            this.currentColorPanel.Size = new System.Drawing.Size(54, 50);
            this.currentColorPanel.TabIndex = 2;
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(422, 12);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(75, 23);
            this.processButton.TabIndex = 3;
            this.processButton.Text = "Process";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // palettePanel
            // 
            this.palettePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.palettePanel.AutoScroll = true;
            this.palettePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.palettePanel.Location = new System.Drawing.Point(734, 112);
            this.palettePanel.Name = "palettePanel";
            this.palettePanel.Size = new System.Drawing.Size(54, 310);
            this.palettePanel.TabIndex = 4;
            // 
            // gridWidthTextBox
            // 
            this.gridWidthTextBox.BeepOnError = true;
            this.gridWidthTextBox.Location = new System.Drawing.Point(211, 12);
            this.gridWidthTextBox.Mask = "00000";
            this.gridWidthTextBox.Name = "gridWidthTextBox";
            this.gridWidthTextBox.PromptChar = ' ';
            this.gridWidthTextBox.Size = new System.Drawing.Size(52, 22);
            this.gridWidthTextBox.TabIndex = 5;
            this.gridWidthTextBox.Text = "100";
            this.gridWidthTextBox.ValidatingType = typeof(int);
            // 
            // labelGridWidth
            // 
            this.labelGridWidth.AutoSize = true;
            this.labelGridWidth.Location = new System.Drawing.Point(126, 15);
            this.labelGridWidth.Name = "labelGridWidth";
            this.labelGridWidth.Size = new System.Drawing.Size(79, 17);
            this.labelGridWidth.TabIndex = 6;
            this.labelGridWidth.Text = "Grid Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Grid Color:";
            // 
            // panelGridColor
            // 
            this.panelGridColor.BackColor = System.Drawing.Color.Black;
            this.panelGridColor.Location = new System.Drawing.Point(361, 12);
            this.panelGridColor.Name = "panelGridColor";
            this.panelGridColor.Size = new System.Drawing.Size(55, 23);
            this.panelGridColor.TabIndex = 8;
            this.panelGridColor.Click += new System.EventHandler(this.panelGridColor_Click);
            // 
            // buttonTextPattern
            // 
            this.buttonTextPattern.Location = new System.Drawing.Point(503, 12);
            this.buttonTextPattern.Name = "buttonTextPattern";
            this.buttonTextPattern.Size = new System.Drawing.Size(105, 23);
            this.buttonTextPattern.TabIndex = 9;
            this.buttonTextPattern.Text = "Text Pattern";
            this.buttonTextPattern.UseVisualStyleBackColor = true;
            this.buttonTextPattern.Click += new System.EventHandler(this.buttonTextPattern_Click);
            // 
            // buttonImagePattern
            // 
            this.buttonImagePattern.Location = new System.Drawing.Point(614, 12);
            this.buttonImagePattern.Name = "buttonImagePattern";
            this.buttonImagePattern.Size = new System.Drawing.Size(113, 23);
            this.buttonImagePattern.TabIndex = 10;
            this.buttonImagePattern.Text = "Image Pattern";
            this.buttonImagePattern.UseVisualStyleBackColor = true;
            this.buttonImagePattern.Click += new System.EventHandler(this.buttonImagePattern_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hintLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 425);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 25);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // hintLabel
            // 
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(148, 20);
            this.hintLabel.Text = "Load image to begin";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonImagePattern);
            this.Controls.Add(this.buttonTextPattern);
            this.Controls.Add(this.panelGridColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelGridWidth);
            this.Controls.Add(this.gridWidthTextBox);
            this.Controls.Add(this.palettePanel);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.currentColorPanel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.loadImageButton);
            this.MinimumSize = new System.Drawing.Size(818, 300);
            this.Name = "MainForm";
            this.Text = "Cornifier";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadImageButton;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel currentColorPanel;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.FlowLayoutPanel palettePanel;
        private System.Windows.Forms.MaskedTextBox gridWidthTextBox;
        private System.Windows.Forms.Label labelGridWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelGridColor;
        private System.Windows.Forms.Button buttonTextPattern;
        private System.Windows.Forms.Button buttonImagePattern;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel hintLabel;
    }
}

