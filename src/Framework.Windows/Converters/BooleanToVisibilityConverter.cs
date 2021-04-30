using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public enum Invisibility
  {
    Hidden = Visibility.Hidden,
    Collapsed = Visibility.Collapsed
  }

  public class BooleanToVisibilityConverter : IValueConverter
  {
    public Invisibility Invisibility { get; set; } = Invisibility.Collapsed;
    public bool Negate { get; set; } = false;

    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var flag = false;
      if (value is bool b)
        flag = b;
      else if (value is bool?)
      {
        var nullable = (bool?)value;
        flag = nullable.HasValue && nullable.Value;
      }

      if (Negate)
        flag = !flag;
      return (Visibility)(flag ? 0 : Invisibility);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return Binding.DoNothing;
    }

    #endregion
  }
}

