using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Framework.Extensions
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Checks whether a given collection contains an item that matches
    /// the submitted predicate.
    /// </summary>
    /// <typeparam name="T">The collection's content.</typeparam>
    /// <param name="source">The sequence to be analyzed.</param>
    /// <param name="predicate">A function that tests each element for a
    /// given condition.</param>
    /// <returns>True if an item was found that matches the submitted
    /// <paramref name="predicate"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="predicate"/>
    /// is a null reference.</exception>
    public static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class
    {
      if (predicate == null) 
        throw new ArgumentNullException(nameof(predicate));
      return source.FirstOrDefault(predicate) != null;
    }

    /// <summary>
    /// Triggers asynchronous invocation of a given action on every
    /// item of a given sequence using AsParallel().ForAll.
    /// </summary>
    /// <typeparam name="T">The collection's content.</typeparam>
    /// <param name="source">The sequence to be processed.</param>
    /// <param name="action">An action delegate that is being invoked
    /// for every item of the <paramref name="source"/> sequence.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="action"/>
    /// is a null reference.</exception>
    public static void DoAsync<T>(this IEnumerable<T> source, Action<T> action)
    {
      if (action == null)
        throw new ArgumentNullException("action");

      source.AsParallel().ForAll(action);
    }

    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childrenFunc)
    {
      return items.SelectMany(node => Traverse(node, childrenFunc));
    }

    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IEnumerable<T> Traverse<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
    {
      yield return head;
      foreach (var child in childrenFunc(head).SelectMany(node => Traverse(node, childrenFunc)))
      {
        yield return child;
      }
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
      return !source.Any();
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
      return source == null || !source.Any();
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumeration)
    {
      return enumeration ?? Enumerable.Empty<T>();
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
    {
      return source.Where(item => item != null);
    }

    /// <summary>
    /// Wraps this object instance into an IEnumerable&lt;T&gt;
    /// consisting of a single item.
    /// </summary>
    /// <typeparam name="T"> Type of the object. </typeparam>
    /// <param name="item"> The instance that will be wrapped. </param>
    /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
    public static IEnumerable<T> Enumerate<T>(this T item)
    {
      yield return item;
    }
  }
}