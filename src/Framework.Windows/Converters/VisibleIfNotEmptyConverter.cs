using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Framework.Windows.Converters
{
  public class VisibleIfNotEmptyConverter : ValueConverter<IEnumerable, Visibility>
  {
    protected override Visibility Convert(
      IEnumerable list,
      object parameter,
      CultureInfo culture)
    {
      return list != null && list.Cast<object>().Any() ? Visibility.Visible : Visibility.Collapsed;
    }
  }
}
