using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class SetFocusAction : TargetedTriggerAction<UIElement>
  {
    protected override void Invoke(object parameter)
    {
      if (Target != null)
        Target.Focus();
    }
  }
}