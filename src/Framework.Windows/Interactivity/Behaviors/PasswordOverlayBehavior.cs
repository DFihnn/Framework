using System.Windows;
using System.Windows.Controls;
using Framework.Windows.Interactivity.Behaviors;
using Framework.Extensions;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class PasswordOverlayBehavior : OverlayBehavior<PasswordBox>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.PasswordChanged += OnPasswordChanged;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
    {
      ShowOverlay = string.IsNullOrEmpty(AssociatedObject.Password);
    }
  }
}