using System;
using System.Globalization;
using Xamarin.Forms;

namespace NextBus.Converters
{
    public class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var distance = (double?) value;
            if (distance == null)
                return "";

            if (distance < 1000)
                return $"{Math.Round(distance.Value, 0)}m away";

            if (distance < 2500)
                return $"{Math.Round(distance.Value/1000, 2)}km away";

            if (distance < 9999)
                return $"{Math.Round(distance.Value / 1000)}km away";

            return "A long way away";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
