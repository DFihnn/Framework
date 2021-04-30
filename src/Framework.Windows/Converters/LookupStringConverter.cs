using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Framework.Windows.Converters
{
  public class LookupStringConverter : IValueConverter
  {
    private readonly Dictionary<string, string> dictionary;

    public LookupStringConverter(Dictionary<string, string> dictionary)
    {
      this.dictionary = dictionary;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        if (dictionary.TryGetValue(value.ToString(), out var text))
          return text;
      }
      return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
