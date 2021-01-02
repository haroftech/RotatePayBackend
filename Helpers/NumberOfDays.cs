using System;

namespace Backend.Helpers
{
    public class NumberOfDays
    {        
        //private NumberOfDays() { }

        public static string getNumberOfDays(DateTime endDate) {
            DateTime userLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            userLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));            
            var ts = new TimeSpan(endDate.Ticks - userLocalTime_Nigeria.Ticks);
            var daysLeft = ts.Days + 1;
            return daysLeft.ToString();
        }  
    }
}
