using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Framework.Extensions
{
  public static class CollectionExtensions
  {
    public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> filter)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof(collection));
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      var items = collection.Select((item, index) => new {item, index}).Where(x => filter(x.item));

      if (collection is IList<T> list)
      {
        var toRemove = items.Select(x => x.index).OrderByDescending(i => i).ToArray();
        foreach (var item in toRemove)
          list.RemoveAt(item);
      }
      else
      {
        var toRemove = items.Select(x => x.item).ToArray();
        foreach (var item in toRemove)
          collection.Remove(item);
      }
    }

    public static void RemoveFirstWhere<T>(this ICollection<T> collection, Func<T, bool> filter)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof(collection));
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      var firstItem = collection.Select((item, index) => new {item, index}).FirstOrDefault(x => filter(x.item));
      if (firstItem == null)
        return;

      if (collection is IList<T> list)
      {
        list.RemoveAt(firstItem.index);
      }
      else
      {
        collection.Remove(firstItem.item);
      }
    }

    public static void AddRange<T>(this Stack<T> stack, IEnumerable<T> source)
    {
      foreach (var t in source)
      {
        stack.Push(t);
      }
    }

    public static bool TryAddRange<T>(this BlockingCollection<T> collection, IEnumerable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      if (collection == null || collection.IsCompleted)
        return false;

      try
      {
        foreach (var t in source)
        {
          if (!collection.TryAdd(t))
            return false;
        }
        return true;
      }
      catch (ObjectDisposedException)
      {
        return false;
      }
      catch (InvalidOperationException)
      {
        return false;
      }
    }

    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> source)
    {
      foreach (var t in source)
      {
        collection.Add(t);
      }
    }

    public static bool EqualContentTo<T>(this List<T> list, IList<T> compareTo)
    {
      var firstNotSecond = list.Except(compareTo);
      var secondNotFirst = compareTo.Except(list);
      return !firstNotSecond.Any() && !secondNotFirst.Any();
    }
  }
}
