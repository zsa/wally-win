﻿using System;
using System.Windows;
using System.Globalization;
using Wally.Models;

namespace Wally.Converters
{
    public class ResetButtonVisibilityConverter : BaseValueConverter<ResetButtonVisibilityConverter>
    {
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
    }
}
