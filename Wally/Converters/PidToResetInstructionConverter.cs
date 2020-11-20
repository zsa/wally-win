using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Wally.Converters
{
    public class PidToResetInstructionConverter : BaseValueConverter<PidToResetInstructionConverter>
    {
        private const string ErgodoxInstruction = "The reset button is located on the right half of your keyboard, next to the three LEDs.";
        private const string PlanckInstruction = "The reset button is located at the top left of the back of your keyboard.";
        private const string MoonlanderInstruction = "The reset button is located on the left half of your keyboard, next to the three LEDs.";

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value switch
            {
                0x1307 or
                0x4974 or
                0x4975 or
                0x4976 or
                0x0478 => ErgodoxInstruction,
                0x6060 or
                0xC6CE or
                0xC6CF => PlanckInstruction,
                0x1969 => MoonlanderInstruction,
                0xDF11 => String.Empty,
                _ => String.Empty 
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
