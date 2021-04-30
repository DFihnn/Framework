using System;
using System.Collections.Generic;

namespace Framework.Extensions
{
  public static class StringExtensions
  {
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
      return source?.IndexOf(toCheck, comp) >= 0;
    }

    public static string Join(this IEnumerable<string> values, string separator)
    {
      return string.Join(separator, values);
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
      return string.IsNullOrWhiteSpace(value);
    }
  }
}
