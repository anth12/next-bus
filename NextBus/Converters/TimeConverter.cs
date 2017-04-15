using System;
using System.Globalization;
using Xamarin.Forms;

namespace NextBus.Converters
{
    public class TimeConverter : IValueConverter
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTime) value;

            if (time == DateTime.MinValue)
                return "";

            
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - time.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);


            if (delta < 1*MINUTE)
                return "Last updated: " + (ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago");

            if (delta < 2*MINUTE)
                return "Last updated: a minute ago";

            if (delta < 45*MINUTE)
                return "Last updated: " + ts.Minutes + " minutes ago";

            if (delta < 90*MINUTE)
                return "Last updated: " + "an hour ago";

            return "Last updated: a while ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
