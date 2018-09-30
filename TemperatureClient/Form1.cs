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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var wu = new WeatherUnderground("brighton");

            var info = await wu.ReadWeatherInformation(new DateTime(2018, 9, 12));

            MessageBox.Show(info.AverageDaytimeDegrees.ToString());
        }
    }
}
