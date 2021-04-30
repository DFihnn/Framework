using System;
using System.Globalization;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public sealed class NegateBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is bool))
        throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Value not of {0}", (object)typeof(bool).FullName));

      if (!targetType.IsAssignableFrom(typeof(bool)))
        throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Target not extending {0}", (object)typeof(bool).FullName));

      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value, targetType, parameter, culture);
    }
  }
}
