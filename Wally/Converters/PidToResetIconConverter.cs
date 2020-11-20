using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Wally.Converters
{
    class PidToResetIconConverter : BaseValueConverter<PidToResetIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = (int)value switch
            {
                0x1307 or
                0x4974 or
                0x4975 or
                0x4976 or
                0x0478 => "ergodox-reset.png",
                0x6060 or
                0xC6CE or
                0xC6CF => "planck-reset.png",
                0x1969 => "moonlander-reset.png",
                0xDF11 => "search.png",
                _ => ""
            };
            return new BitmapImage(new Uri($"/Resources/{image}", UriKind.Relative));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
