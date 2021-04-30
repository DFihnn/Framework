using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class IsBusyBehavior : Behavior<FrameworkElement>
  {
    /// <summary>
    /// IsBusy Dependency Property
    /// </summary>
    public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.Register("IsBusy", typeof(bool), typeof(IsBusyBehavior),
            new FrameworkPropertyMetadata((bool)false,
                new PropertyChangedCallback(OnIsBusyChanged)));

    /// <summary>
    /// Gets or sets the IsBusy property.  This dependency property 
    /// indicates ....
    /// </summary>
    public bool IsBusy
    {
      get { return (bool)GetValue(IsBusyProperty); }
      set { SetValue(IsBusyProperty, value); }
    }

    /// <summary>
    /// Handles changes to the IsBusy property.
    /// </summary>
    private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((IsBusyBehavior)d).OnIsBusyChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the IsBusy property.
    /// </summary>
    protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
    {
      var isBusy = (bool) e.NewValue;
      AssociatedObject.Cursor = isBusy ? Cursors.Wait : Cursors.Arrow;
    }
  }
}