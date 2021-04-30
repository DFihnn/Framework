using System.Globalization;
using System.Windows;

namespace Framework.Windows.Converters
{
  public class VisibleIfNotNullOrEmptyConverter : ValueConverter<string, Visibility>
  {
    protected override Visibility Convert(string value, object parameter, CultureInfo culture)
    {
      var visibility = Visibility.Collapsed;
      if (parameter != null)
        visibility = (Visibility)parameter;
      return !string.IsNullOrEmpty(value) ? Visibility.Visible : visibility;
    }
  }
}
