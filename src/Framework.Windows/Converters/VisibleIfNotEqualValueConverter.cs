using System;
using System.Globalization;
using System.Windows;

namespace Framework.Windows.Converters
{
  public class VisibleIfNotEqualValueConverter : ValueConverter<object, Visibility>
  {
    public VisibleIfNotEqualValueConverter()
    {
      VisibilityIfEqual = Invisibility.Collapsed;
    }

    public Invisibility VisibilityIfEqual { get; set; }

    protected override Visibility Convert(
      object value,
      object parameter,
      CultureInfo culture)
    {
      return Equals(value, parameter) || value != null && parameter != null && (value.GetType() != parameter.GetType() && (StringIsEquivalentToOtherType(value, parameter) || StringIsEquivalentToOtherType(parameter, value)) || value.Equals(parameter)) ? (Visibility)VisibilityIfEqual : Visibility.Visible;
    }

    private bool StringIsEquivalentToOtherType(object stringObj, object otherObj)
    {
      if (!(stringObj is string str))
        return false;
      try
      {
        object type = ((IConvertible)str).ToType(otherObj.GetType(), CultureInfo.InvariantCulture);
        return type == null ? otherObj == null : type.Equals(otherObj);
      }
      catch
      {
        return false;
      }
    }
  }
}
