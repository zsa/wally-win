using System;
using System.Globalization;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the Keyboard reset instructions seen in the <see cref="BootloaderSearchPage"/> based on the Product Id passed.
    /// </summary>
    public class PidToResetInstructionConverter : BaseValueConverter<PidToResetInstructionConverter>
    {
        #region private members
        private const string ErgodoxInstruction = "The reset button is located on the right half of your keyboard, next to the three LEDs.";
        private const string PlanckInstruction = "The reset button is located at the top left of the back of your keyboard.";
        private const string MoonlanderInstruction = "The reset button is located on the left half of your keyboard, next to the three LEDs.";
        #endregion

        #region public members
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
        #endregion
    }
}
