using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class ScrollViewerScrollToTop : TriggerAction<ScrollViewer>
  {
    protected override void Invoke(object parameter)
    {
      AssociatedObject.ScrollToTop();
    }
  }
}
