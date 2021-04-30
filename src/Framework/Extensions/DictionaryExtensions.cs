using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Framework.Extensions
{
  public static class DictionaryExtensions
  {

    public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> filter)
    {
      if (dictionary == null)
        throw new ArgumentNullException(nameof(dictionary));
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      var items = dictionary.Where(filter).ToArray();

      foreach (var item in items)
        dictionary.Remove(item.Key);
    }

    public static void RemoveFirst<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> filter)
    {
      if (dictionary == null)
        throw new ArgumentNullException(nameof(dictionary));
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      var item = dictionary.FirstOrDefault(filter);

      dictionary.Remove(item.Key);
    }

    public static TV GetOrDefault<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
    {
      dictionary.TryGetValue(key, out var v);
      return v;
    }

    public static void AddRange<TKey, TValue>(
      this IDictionary<TKey, TValue> target,
      IDictionary<TKey, TValue> source,
      bool forceUpdate = true)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
      foreach (var keyValuePair in source)
      {
        if (forceUpdate || !target.ContainsKey(keyValuePair.Key))
          target[keyValuePair.Key] = keyValuePair.Value;
      }
    }

    public static void Remove<TK, TV>(this ConcurrentDictionary<TK, TV> dictionary, TK key)
    {
      ((IDictionary<TK, TV>)dictionary).Remove(key);
    }

    public static IDictionary<TK, TV> Copy<TK, TV>(this IDictionary<TK, TV> source, IEqualityComparer<TK> keyComparer, Func<TV, TV> valueCopier)
    {
      var dictionary = new Dictionary<TK, TV>(source.Count, keyComparer);
      foreach (var keyValuePair in source)
        dictionary.Add(keyValuePair.Key, valueCopier(keyValuePair.Value));
      return dictionary;
    }

    public static ReadOnlyDictionary<TK, TV> CopyToReadOnly<TK, TV>(this IDictionary<TK, TV> source, IEqualityComparer<TK> keyComparer, Func<TV, TV> valueCopier)
    {
      return new ReadOnlyDictionary<TK, TV>(source.Copy(keyComparer, valueCopier));
    }
  }
}
