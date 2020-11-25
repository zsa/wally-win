using System;
using System.Globalization;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the "copy logs" buttons labels depending
    /// on <see cref="StateViewModel"/> <see cref="CopiedToClipboard"/> boolean
    /// </summary>
    public class LogsCopiedLabelConverter : BaseValueConverter<LogsCopiedLabelConverter>
    {
        #region public members
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var copied = (bool)value;
            return copied == true ? "Logs copied" : "Copy the logs";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
