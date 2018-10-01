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
        private readonly IUserInteraction userInteraction;
        private const string observationTableXPath = "//div[contains(@class, 'observation-table')]//table";
        private const string summaryTableXPath = "//div[contains(@class, 'summary-table')]//table";


        public WeatherUnderground(string location, IUserInteraction userInteraction)
        {
            this.location = location;
            this.userInteraction = userInteraction;
        }

        private bool IsDocumentFullyLoaded(HtmlDocument htmlDocument, out int remaining)
        {
            bool readingsComplete = GetReadings(htmlDocument, DateTime.Today, out List<WeatherReading> readings, out int readingsRemaining);
            bool daytimeComplete = GetDaytimeInformation(htmlDocument, DateTime.Today, out DateTime sunrise, out DateTime sunset, out int daytimeRemaining);
            remaining = readingsRemaining + daytimeRemaining;
            return readingsComplete && daytimeComplete;

        }

        private bool GetReadings(HtmlDocument htmlDocument, DateTime date, out List<WeatherReading> readings, out int remaining)
        {
            var observationTable = htmlDocument.DocumentNode.SelectSingleNode(observationTableXPath);

            if (observationTable == null)
            {
                readings = null;
                remaining = 24;
                return false;
            }

            readings = new List<WeatherReading>();
            remaining = 0;

            foreach (var row in observationTable.SelectNodes("tbody/tr"))
            {
                var cells = row.SelectNodes("td")?.ToList();

                if (cells == null)
                {
                    remaining++;
                }
                else
                {
                    var timeText = cells[0].InnerText.Trim();
                    var temperatureText = cells[1].InnerText.Trim();
                    timeText = Regex.Match(timeText, @"\d\d:\d\d:\d\d").Value;
                    temperatureText = Regex.Match(temperatureText, @"[\d]+").Value;

                    if (String.IsNullOrWhiteSpace(timeText))
                    {
                        remaining++;
                    }
                    else
                    {
                        var readingTime = date.Date + DateTime.ParseExact(timeText, "HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay;
                        var readingTemperature = int.Parse(temperatureText);

                        readings.Add(WeatherReading.FromFarenheit(readingTime, readingTemperature));
                    }
                }
            }

            return remaining == 0 && readings.Any();
        }

        private static DateTime ParseSunTime(DateTime date, string timeText)
        {
            return date.Date + DateTime.ParseExact(timeText, "h:m tt", CultureInfo.InvariantCulture).TimeOfDay;
        }

        private bool GetDaytimeInformation(HtmlDocument htmlDocument, DateTime date, out DateTime sunrise, out DateTime sunset, out int remaining)
        {
            var summaryTable = htmlDocument.DocumentNode.SelectSingleNode(summaryTableXPath);

            var timeCells = summaryTable?.SelectSingleNode("//th[text() = 'Actual Time']")?.ParentNode.SelectNodes("td")?.ToList();

            if (timeCells == null)
            {
                sunrise = default(DateTime);
                sunset = default(DateTime);
                remaining = 2;
                return false;
            }
            else
            {
                var sunriseText = timeCells[1].InnerText.Trim();
                var sunsetText = timeCells[2].InnerText.Trim();
                remaining = 0;
                sunrise = ParseSunTime(date, sunriseText);
                sunset = ParseSunTime(date, sunsetText);
                return true;
            }
        }


        public async Task<WeatherInformation> ReadWeatherInformation(DateTime date)
        {
            string url = $"https://www.wunderground.com/history/daily/gb/{location}/EGKA/date/{date.ToString("yyyy-M-d")}";

            var htmlDocument = await WebBrowserRequest.Navigate(url, IsDocumentFullyLoaded, this.userInteraction);

            if (GetReadings(htmlDocument, date, out List<WeatherReading> readings, out int dummy)
                && GetDaytimeInformation(htmlDocument, date, out DateTime sunrise, out DateTime sunset, out int dummy2))
            {
                return new WeatherInformation(
                    sunrise,
                    sunset,
                    readings);

            }
            else
            {
                return null;
            }
        }
    }
}
