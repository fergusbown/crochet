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
                    this.log.AppendText(message + Environment.NewLine);
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var wu = new WeatherUnderground(textBoxLocation.Text, new UserInteraction(this.textBoxLog));

            var info = await wu.ReadWeatherInformation(new DateTime(2018, 9, 12));

            MessageBox.Show(info?.AverageDaytimeDegrees.ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TemperatureClient.Properties.Settings.Default.Save();
        }
    }
}
