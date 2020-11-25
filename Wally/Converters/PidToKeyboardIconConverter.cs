using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Wally.Converters
{
    /// <summary>
    /// This converter returns the Keyboard icon seen in the <see cref="KeyboardSelectPage"/> based on the Product Id passed.
    /// </summary>
    public class PidToKeyboardIconConverter : BaseValueConverter<PidToKeyboardIconConverter>
    {
        #region public members
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = (int)value switch
            {
                        0x1307 or 
                        0x4974 or
                        0x4975 or
                        0x4976 or
                        0x0478 => "ergodox-logo.png",
                        0x6060 or
                        0xC6CE or
                        0xC6CF => "planck-logo.png",
                        0x1969 => "moonlander-logo.png",
                        0xDF11 => "search.png",
                        _ => ""
            };
            return new BitmapImage(new Uri($"/Resources/{image}", UriKind.Relative));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
