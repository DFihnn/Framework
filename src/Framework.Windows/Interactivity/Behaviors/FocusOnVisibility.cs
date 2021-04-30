using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class FocusOnVisibility : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      AssociatedObject.IsVisibleChanged += AssociatedObjectIsVisibleChanged;

      if (AssociatedObject.Visibility == Visibility.Visible)
        AssociatedObject.Focus();
    }

    protected override void OnDetaching()
    {
      AssociatedObject.IsVisibleChanged -= AssociatedObjectIsVisibleChanged;
    }

    private void AssociatedObjectIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (AssociatedObject.Visibility == Visibility.Visible)
        AssociatedObject.Focus();
    }
  }
}