using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class ListBoxScrollIntoViewAction : TargetedTriggerAction<ListBox>
  {
    /// <summary>
    /// Item Dependency Property
    /// </summary>
    public static readonly DependencyProperty ItemProperty =
      DependencyProperty.Register("Item",
                                  typeof(object),
                                  typeof(ListBoxScrollIntoViewAction),
                                  new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Gets or sets the Item property.  This dependency property indicates the item to be brought into view.
    /// </summary>
    public object Item
    {
      get { return GetValue(ItemProperty); }
      set { SetValue(ItemProperty, value); }
    }

    protected override void Invoke(object parameter)
    {
      Target.ScrollIntoView(Item);
    }
  }
}