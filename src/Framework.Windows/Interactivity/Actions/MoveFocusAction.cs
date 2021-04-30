using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class MoveFocusAction : TriggerAction<FrameworkElement>
  {
    private FocusNavigationDirection focusNavigationDirection = FocusNavigationDirection.First;

    public FocusNavigationDirection FocusNavigationDirection
    {
      get { return focusNavigationDirection; }
      set { focusNavigationDirection = value; }
    }

    protected override void Invoke(object parameter)
    {
      AssociatedObject.MoveFocus(new TraversalRequest(FocusNavigationDirection));
    }
  }
}
