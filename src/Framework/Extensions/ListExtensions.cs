using System;
using System.Collections.Generic;

namespace Framework.Extensions
{
  public static class ListExtensions
  {
    public static bool RemoveByReference<T>(this IList<T> list, T itemToRemove)
    {
      if (list == null)
        throw new ArgumentNullException(nameof(list));

      int index = 0;
      foreach (var item in list)
      {
        if (ReferenceEquals(item, itemToRemove))
        {
          list.RemoveAt(index);
          return true;
        }
        index++;
      }

      return false;
    }

    public static T LastOrDefault<T>(this IList<T> list)
    {
      if (list == null)
        throw new ArgumentNullException(nameof(list));

      if (list.Count == 0)
        return default(T);

      return list[^1];
    }

  }
}
