using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class FocusWithinBehavior : Behavior<FrameworkElement>
  {
    #region HasFocus

    /// <summary>
    /// HasFocus Dependency Property
    /// </summary>
    public static readonly DependencyProperty HasFocusProperty =
        DependencyProperty.Register("HasFocus", typeof(bool), typeof(FocusWithinBehavior),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets the HasFocus property.  This dependency property 
    /// indicates if the element has IsKeyboardFocusWithin
    /// </summary>
    public bool HasFocus
    {
      get { return (bool)GetValue(HasFocusProperty); }
      set { SetValue(HasFocusProperty, value); }
    }

    #endregion

    protected override void OnAttached()
    {
      AssociatedObject.IsKeyboardFocusWithinChanged += AssociatedObjectOnIsKeyboardFocusWithinChanged;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.IsKeyboardFocusWithinChanged -= AssociatedObjectOnIsKeyboardFocusWithinChanged;
    }

    private void AssociatedObjectOnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      HasFocus = AssociatedObject.IsKeyboardFocusWithin;
    }
  }
}