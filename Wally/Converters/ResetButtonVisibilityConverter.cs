using System;
using System.Windows;
using System.Globalization;
using Wally.Models;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the visibility status of the reset button seen on Wally's
    /// footer section, based on the current <see cref="FlashingStep"/>
    /// </summary>
    public class ResetButtonVisibilityConverter : BaseValueConverter<ResetButtonVisibilityConverter>
    {
        #region public members
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var step = (FlashingStep)value;
            return step == FlashingStep.DisplayLogs || step == FlashingStep.SearchKeyboard || step == FlashingStep.Flash || step == FlashingStep.Complete
                ? Visibility.Collapsed
                : (object)Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
