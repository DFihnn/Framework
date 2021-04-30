using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public sealed class OrBooleanConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var flag = false;
      foreach (var obj in values)
      {
        if (obj == DependencyProperty.UnsetValue)
          flag = true;
        else if ((bool)obj)
          return true;
      }
      return !flag ? false : DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException(nameof(ConvertBack));
    }
  }
}
