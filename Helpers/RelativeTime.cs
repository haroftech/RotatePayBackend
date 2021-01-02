
using System;

namespace Backend.Helpers
{
    public class RelativeTime
    {        
        //private RelativeTime() { }

        public static string getRelativeTime(DateTime inputDate) {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            DateTime userLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            userLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                               

            var ts = new TimeSpan(userLocalTime_Nigeria.Ticks - inputDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            var inputMonth = inputDate.Month;
            var currentMonth = userLocalTime_Nigeria.Month;

            var inputDay = inputDate.Day;
            var currentDay = userLocalTime_Nigeria.Day;

            if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
            return "a minute ago";

            if (delta < 60 * MINUTE)
            return ts.Minutes + " minutes ago";

            if (delta < 120 * MINUTE)
            return "an hour ago";
         
            if (delta < 24 * HOUR) {
                if (currentDay == inputDay) {
                    return ts.Hours + " hours ago";                    
                } else if (currentDay != inputDay) {
                   return "yesterday";
                }                
            }            

            if (delta < 48 * HOUR) {
                if (currentMonth == inputMonth) {
                    if ((currentDay - inputDay) < 2) {
                        return "yesterday";
                    } else if ((currentDay - inputDay) > 1) {
                        return "2 days ago";
                    } 
                } else {
                    if (currentDay == 1) {
                        return "yesterday";
                    } else if (currentDay == 2) {
                        return "2 days ago";
                    }
                }
               
            }

            if (delta < 30 * DAY) {
                if (currentMonth == inputMonth) {
                    if ((currentDay - inputDay) == ts.Days) {
                        return ts.Days + " days ago";
                    } else {
                        return (currentDay - inputDay) + " days ago";
                    }   
                } else {
                    return ts.Days + " days ago";                                  
                }                                 
            }            

            if (delta < 12 * MONTH)
            {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
            }            
        }  
    }
}
