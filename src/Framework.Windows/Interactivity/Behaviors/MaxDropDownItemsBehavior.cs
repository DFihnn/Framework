using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class MaxDropDownItemsBehavior : Behavior<ComboBox>
  {
    public int MaxDropDownItems
    {
      get { return (int)GetValue(MaxDropDownItemsProperty); }
      set { SetValue(MaxDropDownItemsProperty, value); }
    }

    public static readonly DependencyProperty MaxDropDownItemsProperty =
        DependencyProperty.Register("MaxDropDownItems", typeof(int), typeof(MaxDropDownItemsBehavior), new PropertyMetadata(6));
  
    protected override void OnAttached()
    {
      AssociatedObject.DropDownOpened += OnDropDownOpened;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.DropDownOpened -= OnDropDownOpened;
    }

    private void OnDropDownOpened(object sender, EventArgs e)
    {
      AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
      {
        var container = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
        if (container != null && container.RenderSize.Height > 0)
          AssociatedObject.MaxDropDownHeight = (container.RenderSize.Height * MaxDropDownItems) + 2;
      }));
    }
  }
}