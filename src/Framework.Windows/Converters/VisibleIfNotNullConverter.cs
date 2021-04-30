using System.Globalization;
using System.Windows;

namespace Framework.Windows.Converters
{
  public class VisibleIfNotNullConverter : ValueConverter<object, Visibility>
  {
    protected override Visibility Convert(object obj, object parameter, CultureInfo culture)
    {
      var visibility = Visibility.Collapsed;
      if (parameter != null)
        visibility = (Visibility)parameter;
      return obj != null ? Visibility.Visible : visibility;
    }
  }
}
