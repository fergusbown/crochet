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
        private readonly IWebBrowser browser;
        private const string observationTableXPath = "//div[contains(@class, 'observation-table')]//table";
        private const string summaryTableXPath = "//div[contains(@class, 'summary-table')]//table";


        public WeatherUnderground(string location, IUserInteraction userInteraction, IWebBrowser browser)
        {
            this.location = location;
            this.userInteraction = userInteraction;
            this.browser = browser;
        }

        private bool IsDocumentFullyLoaded(HtmlDocument htmlDocument, out int remaining)
        {
            bool readingsComplete = GetReadings(htmlDocument, DateTime.Today, out List<WeatherReading> readings, out int readingsRemaining);
            bool daytimeComplete = GetDaytimeInformation(htmlDocument, DateTime.Today, out DateTime sunrise, out DateTime sunset, out int daytimeRemaining);
            remaining = readingsRemaining + daytimeRemaining;
            return readingsComplete && daytimeComplete;

        }

        private string NullIfEmpty(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? null : value;
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

                    var timeText24h = NullIfEmpty(Regex.Match(timeText, @"\d\d:\d\d:\d\d").Value);
                    var timeText12h = NullIfEmpty(Regex.Match(timeText, @"[\d]+:[\d]+ [A|P]M").Value);

                    temperatureText = Regex.Match(temperatureText, @"[\d]+").Value;

                    if (String.IsNullOrWhiteSpace(timeText24h ?? timeText12h))
                    {
                        remaining++;
                    }
                    else
                    {
                        DateTime readingTime;
                        if (string.IsNullOrEmpty(timeText12h))
                        {
                            readingTime = date.Date + DateTime.ParseExact(timeText, "HH:mm:ss", CultureInfo.InvariantCulture).TimeOfDay;
                        }
                        else
                        {
                            readingTime = ParseSunTime(date.Date, timeText12h);
                        }

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
            GetUrl getUrl = (attempts) =>
            {
                if (attempts % 2 == 0)
                {
                    return $"https://www.wunderground.com/history/daily/gb/{location}/EGKK/date/{date.ToString("yyyy-M-d")}";
                }
                else
                {
                    return $"https://www.wunderground.com/history/daily/gb/{location}/EGKA/date/{date.ToString("yyyy-M-d")}";
                }
            };

            var htmlDocument = await WebBrowserRequest.Navigate(getUrl, IsDocumentFullyLoaded, this.userInteraction, this.browser);

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
