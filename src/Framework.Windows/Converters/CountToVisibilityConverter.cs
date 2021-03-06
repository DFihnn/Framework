﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public class CountToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        var count = (int)value;

        return count > 0 ? Visibility.Visible : Visibility.Collapsed;
      }

      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}