using System;
using System.Globalization;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public class RadioButtonCheckedConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return parameter == null;
      return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
    }
  }
}
