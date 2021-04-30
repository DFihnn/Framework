using System;
using System.Drawing;

namespace Framework.Extensions
{
  public static class Extensions
  {
    public static T GetValueIfCreated<T>(this Lazy<T> lazy) where T : class
    {
      return lazy.IsValueCreated ? lazy.Value : default(T);
    }

    public static string ToDimensionString(this Size size)
    {
      return size.IsEmpty ? size.ToString() : $"{size.Width}x{size.Height}";
    }

    public static bool IsBetween(this double value, double a, double b)
    {
      return a >= b ? b <= value && value <= a : a <= value && value <= b;
    }

  }
}
