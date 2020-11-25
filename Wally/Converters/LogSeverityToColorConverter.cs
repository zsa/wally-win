using System;
using System.Globalization;
using UtilsLibrary;
using System.Windows.Media;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the color of the <see cref="LogSeverity"/> status of a <see cref="Log"/>
    /// entry listed in the <see cref="LogsPage"/> page.
    /// </summary>
    public class LogSeverityToColorConverter : BaseValueConverter<LogSeverityToColorConverter>
    {
        #region private members
        private const string InfoColor = "#FF33B5E1";
        private const string ErrorColor = "#FFEF5253";
        private const string WarningColor = "#FFFF6200";
        #endregion

        #region public members
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
        #endregion
    }
}
