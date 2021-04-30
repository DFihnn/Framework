using System;
using System.Globalization;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public class ToEnumConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Enum.ToObject(targetType, value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
