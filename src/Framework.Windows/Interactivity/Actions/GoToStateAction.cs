using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class GoToStateAction : TargetedTriggerAction<FrameworkElement>
  {
    public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName", typeof(string), typeof(GoToStateAction), new PropertyMetadata(string.Empty));

    /// <summary>
    /// New state name to transition to
    /// </summary>
    public string StateName
    {
      get => (string)GetValue(StateNameProperty);
      set => SetValue(StateNameProperty, value);
    }

    public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

    /// <summary>
    /// True to use transitions when changing states
    /// </summary>
    public bool UseTransitions
    {
      get => (bool)GetValue(UseTransitionsProperty);
      set => SetValue(UseTransitionsProperty, value);
    }

    protected override void Invoke(object parameter)
    {
      if (Target != null)
      {
        var stateName = StateName;
        if (string.IsNullOrEmpty(stateName) && parameter is string)
          stateName = parameter.ToString();

        // Locate the nearest state group
        if (VisualStateUtilities.TryFindNearestStatefulControl(Target, out var stateControl))
          VisualStateUtilities.GoToState(stateControl, stateName, UseTransitions);
      }
    }
  }
}
