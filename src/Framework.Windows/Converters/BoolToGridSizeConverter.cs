using System;
using System.Windows;
using System.Windows.Data;
using Framework.Extensions;

namespace Framework.Windows.Converters
{
  public class BoolToGridSizeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var v = value as bool?;
      var gridUnitType = v.IsTrue() ? GridUnitType.Star : GridUnitType.Auto;
      return new GridLength(1, gridUnitType);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}