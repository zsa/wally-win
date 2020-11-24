using System;
using System.Globalization;
using Wally.Models;
using Wally.Pages;

namespace Wally.Converters
{
    public class StepToPageConverter : BaseValueConverter<StepToPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (FlashingStep)value switch
            {
                FlashingStep.SearchKeyboard => new KeyboardSearchPage(),
                FlashingStep.SelectKeyboard => new KeyboardSelectPage(),
                FlashingStep.SelectFirmware => new FirmwareSelectPage(),
                FlashingStep.SearchBootloader => new BootloaderSearchPage(),
                FlashingStep.Flash => new FlashingPage(),
                FlashingStep.Complete => new CompletePage(),
                FlashingStep.Error => new ErrorPage(),
                FlashingStep.DisplayLogs => new LogsPage(),
                _ => null
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
