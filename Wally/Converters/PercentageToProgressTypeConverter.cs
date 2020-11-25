using System;
using System.Globalization;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the <see cref="FlashingPage"/> progress bar Indeterminate value based on
    /// the flashing progress.
    /// </summary>
    class PercentageToProgressTypeConverter : BaseValueConverter<PercentageToProgressTypeConverter>
    {
        #region public members
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
        #endregion
    }
}
