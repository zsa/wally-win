using System;
using System.Globalization;

namespace Wally.Converters
{
    class PercentageToProgressTypeConverter : BaseValueConverter<PercentageToProgressTypeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percentage = (int)value;
            if (percentage == 0) return true;
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
