using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class TextBoxSelectAllBehavior : Behavior<TextBox>
  {
    protected override void OnAttached()
    {
      AssociatedObject.GotKeyboardFocus += OnGotKeyboardFocus;
      AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.GotKeyboardFocus -= OnGotKeyboardFocus;
      AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      AssociatedObject.SelectAll();
    }

    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!AssociatedObject.IsKeyboardFocusWithin)
      {
        e.Handled = true;
        AssociatedObject.Focus();
      }
    }
  }
}
