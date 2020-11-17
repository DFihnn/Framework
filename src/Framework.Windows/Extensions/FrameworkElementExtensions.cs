using System.Windows;
using System.Windows.Media;

namespace Framework.Windows.Extensions
{
  public static class FrameworkElementExtensions
  {
    /// <summary>
    /// Recurses up the visual tree looking for closest FrameworkElement with DataContext of specified type.
    /// </summary>
    /// <typeparam name="T">The type of DataContext to search for.</typeparam>
    /// <param name="child">The starting FrameworkElement in the visual tree.</param>
    /// <returns>The closest DataContext of the requested type or null if none found.</returns>
    public static T GetDataContextOfTypeFromClosestVisualParent<T>(this FrameworkElement child) where T : class
    {
      if (child == null)
        return null;

      var parentObject = VisualTreeHelper.GetParent(child) as FrameworkElement;

      if (parentObject == null)
        return null;

      if (parentObject.DataContext is T parent)
      {
        return parent;
      }

      return parentObject.GetDataContextOfTypeFromClosestVisualParent<T>();
    }

    public static bool ContainsPoint(this FrameworkElement control, Point point)
    {
      var descendantBounds = VisualTreeHelper.GetDescendantBounds(control);
      var point1 = control.TranslatePoint(descendantBounds.BottomRight, null);
      var point2 = control.TranslatePoint(descendantBounds.TopLeft, null);
      return point.X >= point2.X && point.X <= point1.X && (point.Y >= point2.Y && point.Y <= point1.Y);
    }
  }
}
