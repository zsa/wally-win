using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wally.Converters
{
    /// <summary>
    /// The Basevalue converter class, implements the IValueConverter
    /// required methods and extends the MarkupExtension class
    /// to easily create value converters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region private members
        private static T Converter = null;
        #endregion

        #region public members
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter ?? (Converter = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion
    }
}
