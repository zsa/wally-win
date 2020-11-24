using System;
using System.Windows;
using System.Globalization;
using Wally.Models;
using UtilsLibrary;
using System.Windows.Media;

namespace Wally.Converters
{
    public class LogSeverityToColorConverter : BaseValueConverter<LogSeverityToColorConverter>
    {
        private const string InfoColor = "#FF33B5E1";
        private const string ErrorColor = "#FFEF5253";
        private const string WarningColor = "#FFFF6200";

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = InfoColor;
            var severity = (LogSeverity)value;
            if (severity == LogSeverity.Error) color = ErrorColor;
            if (severity == LogSeverity.Warning) color = WarningColor;
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
