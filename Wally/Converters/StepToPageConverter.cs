using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
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
                FlashingStep.SearchKeyboard => new KeyboardSearch(),
                FlashingStep.SelectKeyboard => new KeyboardSelect(),
                FlashingStep.SelectFirmware => new FirmwareSelect(),
                FlashingStep.SearchBootloader => new BootloaderSearch(),
                FlashingStep.Flash => new Flashing(),
                FlashingStep.Complete => new Complete(),
                FlashingStep.Error => new Error(),
                FlashingStep.DisplayLogs => new Logs(),
                _ => null
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
