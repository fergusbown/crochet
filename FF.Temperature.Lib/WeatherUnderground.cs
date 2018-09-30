using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public class WeatherUnderground
    {
        private readonly string location;
        private const string observationTableXPath = "//div[contains(@class, 'observation-table')]//table";
        private const string summaryTableXPath = "//div[contains(@class, 'summary-table')]//table";


        public WeatherUnderground(string location)
        {
            this.location = location;
        }

        private bool IsDocumentFullyLoaded(HtmlDocument htmlDocument, out int remaining)
        {
            remaining = 24;

            if (!GetDaytimeInformation(htmlDocument, DateTime.Today, out DateTime sunrise, out DateTime sunset))
            {
                return false;
            }

            List<WeatherReading> readings = GetReadings(htmlDocument, DateTime.Today);

            if (readings == null)
            {
                return false;
            }

            remaining = readings.Where(r => r == null).Count();

            if (remaining > 0)
                return false;

            if (readings.Any())
            {
                return true;
            }

            return false;
        }

        private List<WeatherReading> GetReadings(HtmlDocument htmlDocument, DateTime date)
        {
            var observationTable = htmlDocument.DocumentNode.SelectSingleNode(observationTableXPath);

            if (observationTable == null)
                return null;

            List<WeatherReading> readings = new List<WeatherReading>();

            foreach (var row in observationTable.SelectNodes("tbody/tr"))
            {
                var cells = row.SelectNodes("td")?.ToList();

                if (cells == null)
                {
                    readings.Add(null);
                }
                else
                {
                    var timeText = cells[0].InnerText.Trim();
                    var temperatureText = cells[1].InnerText.Trim();
                    timeText = Regex.Match(timeText, @"\d\d:\d\d:\d\d").Value;
                    temperatureText = Regex.Match(temperatureText, @"[\d]+").Value;

                    if (String.IsNullOrWhiteSpace(timeText))
                    {
                        readings.Add(null);
                    }
                    else
                    {
                        var readingTime = date.Date + DateTime.ParseExact(timeText, "HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay;
                        var readingTemperature = int.Parse(temperatureText);

                        readings.Add(WeatherReading.FromFarenheit(readingTime, readingTemperature));
                    }
                }
            }

            return readings;
        }

        private static DateTime ParseSunTime(DateTime date, string timeText)
        {
            return date.Date + DateTime.ParseExact(timeText, "h:m tt", CultureInfo.InvariantCulture).TimeOfDay;
        }

        private bool GetDaytimeInformation(HtmlDocument htmlDocument, DateTime date, out DateTime sunrise, out DateTime sunset)
        {
            var summaryTable = htmlDocument.DocumentNode.SelectSingleNode(summaryTableXPath);

            var timeCells = summaryTable?.SelectSingleNode("//th[text() = 'Actual Time']")?.ParentNode.SelectNodes("td")?.ToList();

            if (timeCells == null)
            {
                sunrise = default(DateTime);
                sunset = default(DateTime);
                return false;
            }
            else
            {
                var sunriseText = timeCells[1].InnerText.Trim();
                var sunsetText = timeCells[2].InnerText.Trim();

                sunrise = ParseSunTime(date, sunriseText);
                sunset = ParseSunTime(date, sunsetText);
                return true;
            }
        }


        public async Task<WeatherInformation> ReadWeatherInformation(DateTime date)
        {
            string url = $"https://www.wunderground.com/history/daily/gb/{location}/EGKA/date/{date.ToString("yyyy-M-d")}";

            var htmlDocument = await WebBrowserRequest.Navigate(url, IsDocumentFullyLoaded);

            GetDaytimeInformation(htmlDocument, date, out DateTime sunrise, out DateTime sunset);

            return new WeatherInformation(sunrise, sunset, GetReadings(htmlDocument, date));
        }
    }
}
