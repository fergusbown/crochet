using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public static class SunriseSunset
    {

        private static bool ParseSunTime(DateTime date, string timeText, out DateTime result)
        {
            if (DateTime.TryParseExact(timeText, "h:m:s tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                result = date.Date + result.TimeOfDay;

                result = DateTime.SpecifyKind(result, DateTimeKind.Utc).ToLocalTime();

                return true;
            }
            else
            {
                result = default(DateTime);
                return false;
            }
        }

        public static WeatherInformation GetInformation(IUserInteraction userInteraction, DateTime date, double latitude, double longitude)
        {
            string address = $"https://api.sunrise-sunset.org/json?lng={longitude}&lat={latitude}&date={date.ToString("yyyy-MM-dd")}";

            userInteraction.Log($"Requesting sunrise/set from {address}");

            using (var response = WebRequest.CreateHttp(address).GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string json = reader.ReadToEnd();

                        JObject parsedJson = JObject.Parse(json);
                        string sunriseText = (string)parsedJson["results"]?["sunrise"];
                        string sunsetText = (string)parsedJson["results"]?["sunset"];

                        if (ParseSunTime(date, sunriseText, out DateTime sunrise) &&
                            ParseSunTime(date, sunsetText, out DateTime sunset))
                        {
                            userInteraction.Log($"Retrieved sunrise/set from {address}");
                            return new WeatherInformation(sunrise, sunset, Enumerable.Empty<WeatherReading>());
                        }
                        else
                        {
                            userInteraction.Log($"Could not retrieve1 sunrise/set from {address}");
                            return null;
                        }
                    }
                }
            }

        }
    }
}
