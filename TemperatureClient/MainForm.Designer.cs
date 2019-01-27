namespace TemperatureClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCefBrowser = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolTipLocation = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.amToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.amToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.amToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.sunriseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pmToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pmToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pmToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.pmToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxLog.Location = new System.Drawing.Point(0, 333);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(786, 92);
            this.textBoxLog.TabIndex = 9;
            this.textBoxLog.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePickerTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateTimePickerFrom);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxLocation);
            this.panel1.Controls.Add(this.buttonGo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 43);
            this.panel1.TabIndex = 10;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(384, 14);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(146, 20);
            this.dateTimePickerTo.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "to";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(210, 14);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(146, 20);
            this.dateTimePickerFrom.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Location:";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(536, 12);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 7;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.textBoxResult);
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResults.Size = new System.Drawing.Size(778, 240);
            this.tabPageResults.TabIndex = 0;
            this.tabPageResults.Text = "Results";
            this.tabPageResults.UseVisualStyleBackColor = true;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResult.Location = new System.Drawing.Point(3, 3);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(772, 234);
            this.textBoxResult.TabIndex = 9;
            this.textBoxResult.WordWrap = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCefBrowser);
            this.tabControl.Controls.Add(this.tabPageResults);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 67);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(786, 266);
            this.tabControl.TabIndex = 8;
            // 
            // tabPageCefBrowser
            // 
            this.tabPageCefBrowser.Location = new System.Drawing.Point(4, 22);
            this.tabPageCefBrowser.Name = "tabPageCefBrowser";
            this.tabPageCefBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCefBrowser.Size = new System.Drawing.Size(778, 240);
            this.tabPageCefBrowser.TabIndex = 3;
            this.tabPageCefBrowser.Text = "Browser";
            this.tabPageCefBrowser.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 425);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(786, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(786, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startTimeToolStripMenuItem,
            this.endTimeToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // startTimeToolStripMenuItem
            // 
            this.startTimeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sunriseToolStripMenuItem,
            this.amToolStripMenuItem,
            this.amToolStripMenuItem1,
            this.amToolStripMenuItem2,
            this.amToolStripMenuItem3,
            this.amToolStripMenuItem4});
            this.startTimeToolStripMenuItem.Name = "startTimeToolStripMenuItem";
            this.startTimeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startTimeToolStripMenuItem.Text = "&Start Time";
            // 
            // endTimeToolStripMenuItem
            // 
            this.endTimeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sunsetToolStripMenuItem,
            this.pmToolStripMenuItem,
            this.pmToolStripMenuItem1,
            this.pmToolStripMenuItem2,
            this.pmToolStripMenuItem3,
            this.pmToolStripMenuItem4});
            this.endTimeToolStripMenuItem.Name = "endTimeToolStripMenuItem";
            this.endTimeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.endTimeToolStripMenuItem.Text = "&End Time";
            // 
            // amToolStripMenuItem
            // 
            this.amToolStripMenuItem.Name = "amToolStripMenuItem";
            this.amToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.amToolStripMenuItem.Text = "&4 am";
            this.amToolStripMenuItem.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // amToolStripMenuItem1
            // 
            this.amToolStripMenuItem1.Name = "amToolStripMenuItem1";
            this.amToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.amToolStripMenuItem1.Text = "&5 am";
            this.amToolStripMenuItem1.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // amToolStripMenuItem2
            // 
            this.amToolStripMenuItem2.Name = "amToolStripMenuItem2";
            this.amToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.amToolStripMenuItem2.Text = "&6 am";
            this.amToolStripMenuItem2.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // amToolStripMenuItem3
            // 
            this.amToolStripMenuItem3.Name = "amToolStripMenuItem3";
            this.amToolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.amToolStripMenuItem3.Text = "&7 am";
            this.amToolStripMenuItem3.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // amToolStripMenuItem4
            // 
            this.amToolStripMenuItem4.Name = "amToolStripMenuItem4";
            this.amToolStripMenuItem4.Size = new System.Drawing.Size(180, 22);
            this.amToolStripMenuItem4.Text = "&8 am";
            this.amToolStripMenuItem4.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // sunriseToolStripMenuItem
            // 
            this.sunriseToolStripMenuItem.Name = "sunriseToolStripMenuItem";
            this.sunriseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sunriseToolStripMenuItem.Text = "&Sunrise";
            this.sunriseToolStripMenuItem.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // sunsetToolStripMenuItem
            // 
            this.sunsetToolStripMenuItem.Name = "sunsetToolStripMenuItem";
            this.sunsetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sunsetToolStripMenuItem.Text = "&Sunset";
            this.sunsetToolStripMenuItem.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // pmToolStripMenuItem
            // 
            this.pmToolStripMenuItem.Name = "pmToolStripMenuItem";
            this.pmToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pmToolStripMenuItem.Text = "&4 pm";
            this.pmToolStripMenuItem.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // pmToolStripMenuItem1
            // 
            this.pmToolStripMenuItem1.Name = "pmToolStripMenuItem1";
            this.pmToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.pmToolStripMenuItem1.Text = "&5 pm";
            this.pmToolStripMenuItem1.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // pmToolStripMenuItem2
            // 
            this.pmToolStripMenuItem2.Name = "pmToolStripMenuItem2";
            this.pmToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.pmToolStripMenuItem2.Text = "&6 pm";
            this.pmToolStripMenuItem2.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // pmToolStripMenuItem3
            // 
            this.pmToolStripMenuItem3.Name = "pmToolStripMenuItem3";
            this.pmToolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.pmToolStripMenuItem3.Text = "&7 pm";
            this.pmToolStripMenuItem3.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // pmToolStripMenuItem4
            // 
            this.pmToolStripMenuItem4.Name = "pmToolStripMenuItem4";
            this.pmToolStripMenuItem4.Size = new System.Drawing.Size(180, 22);
            this.pmToolStripMenuItem4.Text = "&8 pm";
            this.pmToolStripMenuItem4.Click += new System.EventHandler(this.EndTime_Click);
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TemperatureClient.Properties.Settings.Default, "StationLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxLocation.Location = new System.Drawing.Point(69, 14);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(135, 20);
            this.textBoxLocation.TabIndex = 8;
            this.textBoxLocation.Text = global::TemperatureClient.Properties.Settings.Default.StationLocation;
            this.toolTipLocation.SetToolTip(this.textBoxLocation, resources.GetString("textBoxLocation.ToolTip"));
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 448);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Temperature Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageResults.ResumeLayout(false);
            this.tabPageResults.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TabPage tabPageCefBrowser;
        private System.Windows.Forms.ToolTip toolTipLocation;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endTimeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sunriseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem amToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem amToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem amToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem amToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem amToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem sunsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pmToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pmToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pmToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem pmToolStripMenuItem4;
    }
}

