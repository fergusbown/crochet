using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public class WeatherUnderground
    {
        private readonly string location;
        private readonly IUserInteraction userInteraction;
        private readonly IWebBrowser browser;
        private readonly ITimeRangeProvider timeRangeProvider;
        private const string observationTableXPath = "//div[contains(@class, 'observation-table')]//table";


        public WeatherUnderground(
            string location, 
            IUserInteraction userInteraction, 
            IWebBrowser browser,
            ITimeRangeProvider timeRangeProvider)
        {
            this.location = location;
            this.userInteraction = userInteraction;
            this.browser = browser;
            this.timeRangeProvider = timeRangeProvider;
        }

        private bool IsDocumentFullyLoaded(HtmlDocument htmlDocument, out int remaining)
        {
            bool readingsComplete = GetReadings(htmlDocument, DateTime.Today, out List<WeatherReading> readings, out int readingsRemaining);
            bool locationComplete = GetLocationInformation(htmlDocument, out double latitude, out double longitude, out int daytimeRemaining);
            remaining = readingsRemaining + daytimeRemaining;
            return readingsComplete && locationComplete;
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
                            if (!ParseSunTime(date.Date, timeText12h, out readingTime))
                            {
                                remaining++;
                            }
                        }

                        var readingTemperature = int.Parse(temperatureText);

                        readings.Add(WeatherReading.FromFarenheit(readingTime, readingTemperature));
                    }
                }
            }

            return remaining == 0 && readings.Any();
        }

        private static bool ParseSunTime(DateTime date, string timeText, out DateTime result)
        {
            if (DateTime.TryParseExact(timeText, "h:m tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                result = date.Date + result.TimeOfDay;
                return true;
            }
            else
            {
                result = default(DateTime);
                return false;
            }
        }

        private bool TryConvertlocationText(string locationText, out double location)
        {
            if (locationText != null)
            {
                double multiplier = 1;

                if (locationText.EndsWith("S") || locationText.EndsWith("W"))
                {
                    multiplier = -1;
                }

                locationText = locationText.TrimEnd(' ', '°', 'N', 'S', 'E', 'W');

                if (double.TryParse(locationText, out location))
                {
                    location *= multiplier;
                    return true;
                }
            }

            location = double.NaN;
            return false;
        }

        private bool GetLocationInformation(HtmlDocument htmlDocument, out double latitude, out double longitude, out int remaining)
        {
            var location = htmlDocument.DocumentNode.SelectSingleNode("//city-header//span[contains(@class, 'subheading')]")?.InnerText;

            if (location == null)
            {
                latitude = double.NaN;
                longitude = double.NaN;
                remaining = 2;
            }
            else
            {
                remaining = 0;

                var latitudeText = NullIfEmpty(Regex.Match(location, @"[\d]+\.[\d]+ °[N|S]").Value);
                var longitudeText = NullIfEmpty(Regex.Match(location, @"[\d]+\.[\d]+ °[W|E]").Value);

                if (!TryConvertlocationText(latitudeText, out latitude))
                {
                    remaining++;
                }

                if (!TryConvertlocationText(longitudeText, out longitude))
                {
                    remaining++;
                }
            }

            return remaining == 0;
        }


        public async Task<WeatherInformation> ReadWeatherInformation(DateTime date)
        {
            GetUrl getUrl = (attempts) =>
            {
                return $"https://www.wunderground.com/history/daily/{location}/date/{date.ToString("yyyy-M-d")}";
            };

            var htmlDocument = await WebBrowserRequest.Navigate(getUrl, IsDocumentFullyLoaded, this.userInteraction, this.browser);

            if (GetReadings(htmlDocument, date, out List<WeatherReading> readings, out int dummy)
                && GetLocationInformation(htmlDocument, out double latitude, out double longitude, out int dummy2))
            {
                WeatherInformation daylight = this.timeRangeProvider.GetInformation(this.userInteraction, date, latitude, longitude);

                if (daylight != null)
                {
                    return new WeatherInformation(
                        daylight.StartTime,
                        daylight.EndTime,
                        readings);
                }
            }

            return null;
        }
    }
}
