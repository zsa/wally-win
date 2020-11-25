using System;
using System.Globalization;

namespace Wally.Converters
{
    public class LogsCopiedLabelConverter : BaseValueConverter<LogsCopiedLabelConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var copied = (bool)value;
            return copied == true ? "Logs copied" : "Copy the logs";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
