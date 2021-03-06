﻿using CefSharp.WinForms;
using FF.Temperature.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemperatureClient
{
    public partial class MainForm : Form
    {
        private readonly ChromiumWebBrowser chromiumBrowser;
        private readonly IWebBrowser browser;
        public MainForm()
        {
            InitializeComponent();
            this.chromiumBrowser = new ChromiumWebBrowser(String.Empty);
            this.tabPageCefBrowser.Controls.Add(this.chromiumBrowser);
            this.chromiumBrowser.Dock = DockStyle.Fill;
            //this.browser = new InvokingWebBrowser(this, new WebBrowserWrapper(this.webBrowser));
            this.browser = new CefBrowserWrapper(this.chromiumBrowser);
            InitializeDates();

            this.toolTipLocation.SetToolTip(
                this.label1,
                this.toolTipLocation.GetToolTip(this.textBoxLocation));

            Application.Idle += Application_Idle;
        }

        private void InitializeDates()
        {
            DateTime endDate = DateTime.Today.AddDays(-1);
            DateTime startDate;
            if (Properties.Settings.Default.LastDate.Ticks > 0)
            {
                startDate = Properties.Settings.Default.LastDate.AddDays(1);
            }
            else
            {
                startDate = endDate;
            }

            if (startDate > endDate)
                startDate = endDate;

            dateTimePickerFrom.Value = startDate;
            dateTimePickerTo.Value = endDate;
        }

        private class UserInteraction : IUserInteraction
        {
            private readonly TextBox log;

            public UserInteraction(TextBox log)
            {
                this.log = log;
            }

            public void Log(string message)
            {
                if (this.log.InvokeRequired)
                {
                    Action invoke = () => this.Log(message);
                    this.log.Invoke(invoke);
                }
                else
                {
                    this.log.AppendText($"({DateTime.Now.ToString("HH:mm:ss")}){message}{Environment.NewLine}");
                }
            }

            public bool ShouldContinue(string message)
            {
                if (this.log.InvokeRequired)
                {
                    Func<bool> invoke = () => this.ShouldContinue(message);
                    return (bool)this.log.Invoke(invoke);
                }
                else
                {
                    return MessageBox.Show(
                        this.log, 
                        message, 
                        "Temperature Data", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning, 
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes;
                }
            }
        }

        private async void buttonGo_Click(object sender, EventArgs e)
        {
            buttonGo.Enabled = false;
            try
            {
                ITimeRangeProvider timeRangeProvider = TimeRangeProviderFactory.GetProvider(
                    Properties.Settings.Default.StartTime,
                    Properties.Settings.Default.EndTime);

                var wu = new WeatherUnderground(textBoxLocation.Text, new UserInteraction(this.textBoxLog), this.browser, timeRangeProvider);
                List<WeatherInformation> weatherInformation = new List<WeatherInformation>();
                DateTime workingDate = dateTimePickerFrom.Value;
                DateTime endDate = dateTimePickerTo.Value;

                int requestCount = (endDate - workingDate).Days + 1;

                this.progressBar1.Maximum = requestCount;

                while (workingDate <= endDate)
                {
                    var info = await wu.ReadWeatherInformation(workingDate);

                    if (info == null)
                    {
                        ProcessResult(weatherInformation);
                        return;
                    }
                    else
                    {
                        weatherInformation.Add(info);
                        workingDate = workingDate.AddDays(1);
                        this.progressBar1.Value++;
                    }
                }

                ProcessResult(weatherInformation);
            }
            finally
            {
                buttonGo.Enabled = true;
                this.progressBar1.Value = 0;
            }
        }

        private void ProcessResult(List<WeatherInformation> weatherInformation)
        {
            if (weatherInformation.Any())
            {
                StringBuilder result = new StringBuilder();

                foreach (var info in weatherInformation)
                {
                    result.AppendLine($"{info.StartTime.Year}-{info.StartTime.Month}-{info.StartTime.Day}\t{info.AverageDegrees}");
                }

                textBoxResult.Text = result.ToString();
                Clipboard.SetText(result.ToString());

                WeatherInformation latest = weatherInformation.LastOrDefault();

                if (latest != null)
                {
                    Properties.Settings.Default.LastDate = latest.StartTime.Date;
                    InitializeDates();
                }

                MessageBox.Show(
                    this,
                    "Copied results to clipboard and results tab",
                    "Temperature Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private int GetTime(object sender)
        {
            string text = ((ToolStripMenuItem)sender).Text.Replace("&", "");

            if (int.TryParse(text.Substring(0, 1), out int hour))
            {
                return hour;
            }
            else
            {
                return 0;
            }
        }

        private void CheckTimeMenu(ToolStripMenuItem parent, int selectedTime)
        {
            foreach (ToolStripMenuItem menuItem in parent.DropDownItems)
            {
                menuItem.Checked = GetTime(menuItem) == selectedTime;
            }
        }

        private void StartTime_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartTime = GetTime(sender);
            Properties.Settings.Default.Save();
        }

        private void EndTime_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.EndTime = GetTime(sender);
            Properties.Settings.Default.Save();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            CheckTimeMenu(startTimeToolStripMenuItem, Properties.Settings.Default.StartTime);
            CheckTimeMenu(endTimeToolStripMenuItem, Properties.Settings.Default.EndTime);
        }

    }
}
