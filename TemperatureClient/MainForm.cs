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
        public MainForm()
        {
            InitializeComponent();

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
                    this.log.AppendText($"({DateTime.Now.TimeOfDay}){message}{Environment.NewLine}");
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
                var wu = new WeatherUnderground(textBoxLocation.Text, new UserInteraction(this.textBoxLog));
                List<WeatherInformation> weatherInformation = new List<WeatherInformation>();
                DateTime workingDate = dateTimePickerFrom.Value;
                DateTime endDate = dateTimePickerTo.Value;

                while (workingDate <= endDate)
                {
                    var info = await wu.ReadWeatherInformation(workingDate);

                    if (info == null)
                    {
                        return;
                    }
                    else
                    {
                        weatherInformation.Add(info);
                        workingDate = workingDate.AddDays(1);
                    }
                }

                ProcessResult(weatherInformation);
            }
            finally
            {
                buttonGo.Enabled = true;
            }

        }

        private void ProcessResult(List<WeatherInformation> weatherInformation)
        {
            StringBuilder result = new StringBuilder();

            foreach (var info in weatherInformation)
            {
                result.AppendLine($"{info.Sunrise.Year}\\{info.Sunrise.Month}\\{info.Sunrise.Day}\t{info.AverageDaytimeDegrees}");
            }

            textBoxResult.Text = result.ToString();
            Clipboard.SetText(result.ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastDate = dateTimePickerTo.Value;
            Properties.Settings.Default.Save();
        }
    }
}
