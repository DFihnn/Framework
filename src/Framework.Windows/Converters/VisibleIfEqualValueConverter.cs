using System;
using System.Globalization;
using System.Windows;

namespace Framework.Windows.Converters
{
  public class VisibleIfEqualValueConverter : ValueConverter<object, Visibility>
  {
    public VisibleIfEqualValueConverter()
    {
      VisibilityIfNotEqual = Invisibility.Collapsed;
    }

    public Invisibility VisibilityIfNotEqual { get; set; }

    protected override Visibility Convert(
      object value,
      object parameter,
      CultureInfo culture)
    {
      return Equals(value, parameter) ||
             value != null && parameter != null &&
             (
               value.Equals(parameter) ||
               value.GetType() != parameter.GetType() &&
               (
                 StringIsEquivalentToOtherType(value, parameter) ||
                 StringIsEquivalentToOtherType(parameter, value)
               )
             ) ? Visibility.Visible : (Visibility)VisibilityIfNotEqual;
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
