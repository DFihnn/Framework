using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class ComboBoxOverlayBehavior : OverlayBehavior<ComboBox>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.AddHandler(TextBoxBase.TextChangedEvent, new RoutedEventHandler(OnTextChanged));
    }

    private void OnTextChanged(object sender, RoutedEventArgs routedEventArgs)
    {
      ShowOverlay = string.IsNullOrEmpty(AssociatedObject.Text);
    }
  }
}

