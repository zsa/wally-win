using System;
using System.Windows;
using System.Globalization;
using Wally.Models;

namespace Wally.Converters
{
    /// <summary>
    /// This Converter returns the visibility of the four progress pills seen at the bottom
    /// of the app to hide them when displaying the logs.
    /// </summary>
    public class StepPillsVisibilityConverter : BaseValueConverter<StepPillsVisibilityConverter>
    {
        #region public members
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var step = (FlashingStep)value;
            if (step == FlashingStep.DisplayLogs) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
