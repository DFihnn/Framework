using System;
using System.Globalization;

namespace Framework.Utilities
{
  public class Reflection
  {
    public static object GetProperty(object source, string propName)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      if (source is DynamicObservableObject dynamicObject)
      {
        if (dynamicObject.TryGetProperty(propName, out var result))
          return result;
      }

      var property = source.GetType().GetProperty(propName);
      return property != null ? property.GetValue(source, null) : null;
    }

    public static bool TryGetProperty(object source, string propName, out object result)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      if (source is DynamicObservableObject dynamicObject)
      {
        if (dynamicObject.TryGetProperty(propName, out result))
          return true;
      }

      var property = source.GetType().GetProperty(propName);
      if (property != null)
      {
        result = property.GetValue(source, null);
        return true;
      }
      result = null;
      return false;
    }

    public static T GetPropertyValueAs<T>(object source, string propName)
    {
      var value = GetProperty(source, propName);

      if (value is T variable)
        return variable;

      if (value == null && typeof(T).IsValueType)
        return default(T);

      var u = Nullable.GetUnderlyingType(typeof(T));

      if (value != null && u != null)
        return (T)Convert.ChangeType(value, u, CultureInfo.CurrentCulture);

      return (T)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);
    }

    public static bool TryGetPropertyValueAs<T>(object source, string propName, out T result)
    {
      if (!TryGetProperty(source, propName, out var value))
      {
        result = default(T);
        return false;
      }

      if (value is T variable)
      {
        result = variable;
        return true;
      }

      if (value == null && typeof(T).IsValueType)
      {
        result = default(T);
        return true;
      }

      var u = Nullable.GetUnderlyingType(typeof(T));

      if (value != null && u != null)
        result = (T)Convert.ChangeType(value, u, CultureInfo.CurrentCulture);
      else
        result = (T)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);
      return true;
    }
  }
}
