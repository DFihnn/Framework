using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
  {
    /// <summary>
    /// Item Dependency Property
    /// </summary>
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register("Item", typeof(object), typeof(ListBoxScrollIntoViewBehavior),
            new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(OnItemChanged)));

    /// <summary>
    /// Gets or sets the Item property.  This dependency property 
    /// indicates ....
    /// </summary>
    public object Item
    {
      get { return (object)GetValue(ItemProperty); }
      set { SetValue(ItemProperty, value); }
    }

    /// <summary>
    /// Handles changes to the Item property.
    /// </summary>
    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ListBoxScrollIntoViewBehavior)d).OnItemChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the Item property.
    /// </summary>
    protected virtual void OnItemChanged(DependencyPropertyChangedEventArgs e)
    {
      AssociatedObject.ScrollIntoView(e.NewValue);
    }
  }
}