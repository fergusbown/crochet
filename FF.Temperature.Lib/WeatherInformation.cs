using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public class WeatherReading
    {
        public DateTime ReadingTime { get; }

        public double Farenheight { get; }

        public double Degrees { get; }

        private WeatherReading(DateTime readingTime, double farenheight, double degrees)
        {
            this.ReadingTime = readingTime;
            this.Farenheight = farenheight;
            this.Degrees = degrees;
        }

        public static WeatherReading FromFarenheit(DateTime readingTime, double farenheight)
        {
            return new WeatherReading(
                readingTime,
                farenheight,
                (farenheight - 32) / 1.8);
        }


        public static WeatherReading FromDegrees(DateTime readingTime, double degrees)
        {
            return new WeatherReading(
                readingTime,
                (degrees * 1.8) + 32,
                degrees);
        }
    }

    public class WeatherInformation
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public IEnumerable<WeatherReading> Readings { get; }

        public WeatherInformation(DateTime startTime, DateTime endTime, IEnumerable<WeatherReading> readings)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Readings = readings.ToList();
        }

        public int AverageDegrees
        {
            get
            {
                var average = this.Readings
                    .Where(r => r.ReadingTime >= this.StartTime && r.ReadingTime <= this.EndTime)
                    .Select(r => r.Degrees)
                    .Average();

                return (int)Math.Round(average);
            }
        }
    }
}
