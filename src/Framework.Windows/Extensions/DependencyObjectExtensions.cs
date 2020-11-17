using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Framework.Windows.Extensions
{
  /// <summary>
  /// Extension methods to dependency objects
  /// </summary>
  public static class DependencyObjectExtensions
  {

    #region Find Ancestor

    /// <summary>
    /// Finds ancestor a of a given item on the visual tree.
    /// </summary>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="obj">A direct or indirect child of the
    /// queried item.</param>
    /// <returns>The first ancestor item that matches the submitted
    /// type parameter. If not matching item can be found, a null
    /// reference is being returned.</returns>
    public static T FindAncestorOfType<T>(this DependencyObject obj) where T : DependencyObject
    {
      if (obj == null)
        return null;

      T correctlyTyped = obj as T;
      if (correctlyTyped != null)
      {
        return correctlyTyped;
      }

      return FindAncestorOfType<T>(GetParentObject(obj));
    }

    public static DependencyObject GetVisualOrLogicalParent(this DependencyObject sourceElement)
    {
      if (sourceElement == null)
        return null;
      return sourceElement is Visual ? VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement) : LogicalTreeHelper.GetParent(sourceElement);
    }

    /// <summary>
    /// This method is an alternative to WPF's
    /// <see cref="VisualTreeHelper.GetParent"/> method, which also
    /// supports content elements. Keep in mind that for content element,
    /// this method falls back to the logical tree of the element!
    /// </summary>
    /// <param name="child">The item to be processed.</param>
    /// <returns>The submitted item's parent, if available. Otherwise
    /// null.</returns>
    public static DependencyObject GetParentObject(this DependencyObject child)
    {
      if (child == null) return null;

      DependencyObject parent = null;
      if (child is Visual)
      {
        parent = VisualTreeHelper.GetParent(child);
      }
      if (parent == null)
      {
        //handle content elements separately
        ContentElement contentElement = child as ContentElement;
        if (contentElement != null)
        {
          parent = ContentOperations.GetParent(contentElement);
          if (parent != null) return parent;

          FrameworkContentElement fce = contentElement as FrameworkContentElement;
          return fce != null ? fce.Parent : null;
        }

        //also try searching for parent in framework elements (such as DockPanel, etc)
        FrameworkElement frameworkElement = child as FrameworkElement;
        if (frameworkElement != null)
        {
          parent = frameworkElement.Parent;
          if (parent != null) return parent;
        }
      }

      return parent;
    }

    public static IEnumerable<DependencyObject> GetAllParentObjects(this DependencyObject child)
    {
      var parent = child.GetParentObject();
      while (parent != null)
      {
        yield return parent;
        parent = parent.GetParentObject();
      }
    }

    #endregion

    #region Find Children

    /// <summary>
    /// Analyzes both visual and logical tree in order to find first elements of a given
    /// type that are descendant of the <paramref name="source"/> item.
    /// </summary>
    /// <typeparam name="T">The type of the queried items.</typeparam>
    /// <param name="source">The root element that marks the source of the search. If the
    /// source is already of the requested type, it will not be included in the result.</param>
    /// <returns>First descendant of <paramref name="source"/> that match the requested type.</returns>
    public static T FindFirstChildOfType<T>(this DependencyObject source) where T : DependencyObject
    {
      IEnumerable<T> x = FindChildrenOfType<T>(source);
      return x.FirstOrDefault();
    }

    /// <summary>
    /// Analyzes both visual and logical tree in order to find all elements of a given
    /// type that are descendants of the <paramref name="source"/> item.
    /// </summary>
    /// <typeparam name="T">The type of the queried items.</typeparam>
    /// <param name="source">The root element that marks the source of the search. If the
    /// source is already of the requested type, it will not be included in the result.</param>
    /// <returns>All descendants of <paramref name="source"/> that match the requested type.</returns>
    public static IEnumerable<T> FindChildrenOfType<T>(this DependencyObject source) where T : DependencyObject
    {
      return FindChildrenOfType<T>(source, false);
    }

    public static IEnumerable<T> FindChildrenOfType<T>(this DependencyObject source, bool ignoreLogicalTree) where T : DependencyObject
    {
      if (source != null)
      {
        var childs = GetChildObjects(source, ignoreLogicalTree);
        foreach (DependencyObject child in childs)
        {
          //analyze if children match the requested type
          if (child != null && child is T)
          {
            yield return (T)child;
          }

          //recurse tree
          foreach (T descendant in FindChildrenOfType<T>(child,ignoreLogicalTree))
          {
            yield return descendant;
          }
        }
      }
    }

    /// <summary>
    /// This method is an alternative to WPF's
    /// <see cref="VisualTreeHelper.GetChild"/> method. 
    /// Keep in mind that for content elements with new visual tree (as Popup),
    /// <see cref="VisualTreeHelper.GetChild"/> does not 
    /// falls back to the logical tree of the element.
    /// </summary>
    /// <param name="parent">The item to be processed.</param>
    /// <returns>The submitted item's child elements, if available.</returns>
    public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
    {
      return GetChildObjects(parent, false);
    }

    public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent, bool ignoreLogicalTree)
    {
      if (parent == null) yield break;

      int count = 0;
      if (parent is Visual)
      {
        //use the visual tree per default
        count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
          yield return VisualTreeHelper.GetChild(parent, i);
        }
      }
      if (count == 0 && !ignoreLogicalTree)
      {
        if ((parent is ContentElement || parent is FrameworkElement))
        {
          //use the logical tree for content / framework elements
          foreach (object obj in LogicalTreeHelper.GetChildren(parent))
          {
            var depObj = obj as DependencyObject;
            if (depObj != null) yield return (DependencyObject)obj;
          }
        }
      }
    }

    /// <summary>
    /// This method is an alternative to recurse call WPF's
    /// <see cref="VisualTreeHelper.GetChild"/> method. 
    /// Keep in mind that for content elements with new visual tree (as Popup),
    /// <see cref="VisualTreeHelper.GetChild"/> does not 
    /// falls back to the logical tree of the element.
    /// </summary>
    /// <param name="parent">The item to be processed.</param>
    /// <returns>The submitted item's child elements, if available.</returns>
    public static IEnumerable<DependencyObject> GetAllChildObjects(this DependencyObject parent)
    {
      foreach (DependencyObject dependencyObject in GetChildObjects(parent))
      {
        yield return dependencyObject;

        //recurse tree
        foreach (DependencyObject descendant in GetAllChildObjects(dependencyObject))
        {
          yield return descendant;
        }
      }
    }

    #endregion

    public static void InvalidateMeasureAllChildObjects(this UIElement parent)
    {
      foreach (var element in parent.GetAllChildObjects().OfType<UIElement>())
      {
        element.InvalidateMeasure();
      }
    }

    public static bool IsConnectedToPresentationSource(this DependencyObject obj)
    {
      return PresentationSource.FromDependencyObject(obj) != null;
    }

    public static DependencyObject FindCommonAncestor(this DependencyObject obj1, DependencyObject obj2)
    {
      return obj1.FindCommonAncestor(obj2, GetVisualOrLogicalParent);
    }

    public static T FindCommonAncestor<T>(this T obj1, T obj2, Func<T, T> parentEvaluator) where T : DependencyObject
    {
      if (obj1 == null || obj2 == null)
        return default(T);
      var objSet = new HashSet<T>();
      for (obj1 = parentEvaluator(obj1); obj1 != null; obj1 = parentEvaluator(obj1))
        objSet.Add(obj1);
      for (obj2 = parentEvaluator(obj2); obj2 != null; obj2 = parentEvaluator(obj2))
      {
        if (objSet.Contains(obj2))
          return obj2;
      }
      return default(T);
    }
  }
}