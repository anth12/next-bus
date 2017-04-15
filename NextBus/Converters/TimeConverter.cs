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
            var result = new FormattedString();

            var time = (DateTime?) value;

            if (time == null)
                return "";

            
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - time.Value.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            result.Spans.Add(new Span {Text = "Last updated " });

            if (delta < 1 * MINUTE)
                result.Spans.Add(new Span {
                    Text = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago"
                });

            else if (delta < 2*MINUTE)
                result.Spans.Add(new Span
                {
                    Text = "a minute ago"
                });

            else if (delta < 45*MINUTE)
                result.Spans.Add(new Span
                {
                    Text = ts.Minutes + " minutes ago"
                });

            else if (delta < 90*MINUTE)
                result.Spans.Add(new Span
                {
                    Text = "an hour ago"
                });

            else if (delta < 24*HOUR)
                result.Spans.Add(new Span
                {
                    Text = ts.Hours + " hours ago"
                });

            else if (delta < 48*HOUR)
                result.Spans.Add(new Span
                {
                    Text = "yesterday"
                });

            else if (delta < 30*DAY)
                result.Spans.Add(new Span
                {
                    Text = ts.Days + " days ago"
                });

            else if (delta < 12*MONTH)
            {
                int months = (int) (Math.Floor((double) ts.Days/30));
                result.Spans.Add(new Span
                {
                    Text = months <= 1 ? "one month ago" : months + " months ago"
                });
            }

            else
            {
                int years = (int) (Math.Floor((double) ts.Days/365));
                result.Spans.Add(new Span
                {
                    Text = years <= 1 ? "one year ago" : years + " years ago"
                });
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
