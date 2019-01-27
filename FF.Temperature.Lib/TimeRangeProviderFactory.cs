using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public static class TimeRangeProviderFactory
    {
        private class FixedTimeProvider : ITimeRangeProvider
        {
            private int startTime;
            private int endTime;

            public FixedTimeProvider(int startTime, int endTime)
            {
                this.startTime = startTime;
                this.endTime = endTime;
            }

            public WeatherInformation GetInformation(IUserInteraction userInteraction, DateTime date, double latitude, double longitude)
            {
                return new WeatherInformation(
                    date.Date.AddHours(startTime),
                    date.Date.AddHours(endTime + 12),
                    Enumerable.Empty<WeatherReading>());
            }
        }

        private class CompositeProvider : ITimeRangeProvider
        {
            private ITimeRangeProvider startTime;
            private ITimeRangeProvider endTime;

            public CompositeProvider(ITimeRangeProvider startTime, ITimeRangeProvider endTime)
            {
                this.startTime = startTime;
                this.endTime = endTime;
            }

            public WeatherInformation GetInformation(IUserInteraction userInteraction, DateTime date, double latitude, double longitude)
            {
                WeatherInformation start = this.startTime.GetInformation(userInteraction, date, latitude, longitude);
                WeatherInformation end = this.endTime.GetInformation(userInteraction, date, latitude, longitude);
                return new WeatherInformation(
                    start.StartTime,
                    end.EndTime,
                    Enumerable.Empty<WeatherReading>());
            }
        }

        public static ITimeRangeProvider GetProvider(int startTime, int endTime)
        {
            if (startTime == 0 && endTime == 0)
            {
                return new SunriseSunset();
            }
            else
            {
                var fixedTimeProvider = new FixedTimeProvider(startTime, endTime);

                if (startTime == 0)
                {
                    return new CompositeProvider(
                        new SunriseSunset(),
                        fixedTimeProvider);
                }
                else if (endTime == 0)
                {
                    return new CompositeProvider(
                        fixedTimeProvider,
                        new SunriseSunset());
                }
                else
                {
                    return fixedTimeProvider;
                }
            }
        }
    }
}
