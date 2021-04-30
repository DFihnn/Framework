using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class TextBoxSelectAllAction : TriggerAction<TextBox>
  {
    protected override void Invoke(object parameter)
    {
      AssociatedObject.SelectAll();
    }
  }
}
