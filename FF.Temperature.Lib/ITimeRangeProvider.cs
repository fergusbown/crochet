using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Temperature.Lib
{
    public interface ITimeRangeProvider
    {
        WeatherInformation GetInformation(IUserInteraction userInteraction, DateTime date, double latitude, double longitude);
    }
}
