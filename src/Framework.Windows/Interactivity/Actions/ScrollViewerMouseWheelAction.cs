using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class ScrollViewerMouseWheelAction : TargetedTriggerAction<ScrollViewer>
  {
    protected override void Invoke(object parameter)
    {
      var eventArgs = parameter as MouseWheelEventArgs;
      if (eventArgs != null)
      {
        for (int mouseWheelScrollLines = 1; mouseWheelScrollLines <= SystemInformation.MouseWheelScrollLines; mouseWheelScrollLines++)
        {
          if (eventArgs.Delta > 0)
            Target.LineUp();
          else if (eventArgs.Delta < 0)
            Target.LineDown();
        }
      }
    }
  }
}
