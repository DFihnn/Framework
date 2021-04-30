using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class ListBoxMoveOnUpDownKeys : TargetedTriggerAction<ListBox>
  {
    protected override void Invoke(object parameter)
    {
      var keyEventArgs = parameter as KeyEventArgs;
      if (keyEventArgs == null)
        return;

      switch (keyEventArgs.Key)
      {
        case Key.Up:
          if (Target.SelectedIndex > 0)
            Target.SelectedIndex--;
          else
            Target.SelectedIndex = Target.Items.Count - 1;          
          break;
        case Key.Down:
          if (Target.SelectedIndex < Target.Items.Count - 1)
            Target.SelectedIndex++;
          else
            Target.SelectedIndex = 0;
          break;
      }
      Target.ScrollIntoView(Target.SelectedItem);
    }
  }
}
