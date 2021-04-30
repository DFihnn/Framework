using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Framework.Extensions;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Actions
{
  public class EventToCommand : TriggerAction<FrameworkElement>
  {
    private IDisposable commandChangedSubscription;

    public bool PassEventArgsToCommand { get; set; }
    public bool AlwaysEnableElement { get; set; }
    
    #region Overrides
    /// <param name="parameter">The EventArgs of the fired event.</param>
    protected override void Invoke(object parameter)
    {
      if (IsAssociatedElementDisabled())
      {
        return;
      }

      ICommand command = Command;
      var commandParameter = CommandParameter;

      if (commandParameter == null && PassEventArgsToCommand)
      {
        commandParameter = parameter;
      }

      if (command != null && command.CanExecute(commandParameter))
      {
        command.Execute(commandParameter);
      }
    }

    /// <summary>
    /// Called when this trigger is attached to a FrameworkElement.
    /// </summary>
    protected override void OnAttached()
    {
      base.OnAttached();
      EnableDisableElement();
    }
    #endregion

    #region Private Methods
    private static void OnCommandChanged(EventToCommand thisTrigger, DependencyPropertyChangedEventArgs e)
    {
      if (thisTrigger == null)
      {
        return;
      }

      thisTrigger.OnCommandChanged(e);
    }

    private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
    {
      if (commandChangedSubscription != null)
        commandChangedSubscription.Dispose();

      var command = e.NewValue as ICommand;
      if (command != null)
      {
        var canExecuteEvent = Observable.FromEventPattern<EventHandler, EventArgs>(ev => command.CanExecuteChanged += ev, ev => command.CanExecuteChanged -= ev);
        commandChangedSubscription = canExecuteEvent.SubscribeWeakly(this, OnSubscribeWeakly);
      }
      EnableDisableElement();
    }

    private static void OnSubscribeWeakly(EventToCommand context, EventPattern<EventArgs> args)
    {
      context.OnCommandCanExecuteChanged(args.Sender, args.EventArgs);
    }

    private bool IsAssociatedElementDisabled()
    {
      return AssociatedObject != null && !AssociatedObject.IsEnabled;
    }

    private void EnableDisableElement()
    {
      if (AssociatedObject == null || Command == null)
      {
        return;
      }
      AssociatedObject.IsEnabled = AlwaysEnableElement || Command.CanExecute(CommandParameter);
    }

    private void OnCommandCanExecuteChanged(object sender, EventArgs e)
    {
      EnableDisableElement();
    }
    #endregion

    #region DPs

    #region CommandParameter
    /// <summary>
    /// Identifies the <see cref="CommandParameter" /> dependency property
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        "CommandParameter", typeof(object), typeof(EventToCommand),
        new PropertyMetadata(null,
            (s, e) =>
            {
              EventToCommand sender = s as EventToCommand;
              if (sender == null)
              {
                return;
              }

              if (sender.AssociatedObject == null)
              {
                return;
              }

              sender.EnableDisableElement();
            }));


    /// <summary>
    /// Gets or sets an object that will be passed to the <see cref="Command" />
    /// attached to this trigger. This is a DependencyProperty.
    /// </summary>
    public object CommandParameter
    {
      get { return GetValue(CommandParameterProperty); }
      set { SetValue(CommandParameterProperty, value); }
    }
    #endregion

    #region Command
    /// <summary
    /// >
    /// Identifies the <see cref="Command" /> dependency property
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        "Command", typeof(ICommand), typeof(EventToCommand),
        new PropertyMetadata(null,
            (s, e) => OnCommandChanged(s as EventToCommand, e)));


    /// <summary>
    /// Gets or sets the ICommand that this trigger is bound to. This
    /// is a DependencyProperty.
    /// </summary>
    public ICommand Command
    {
      get { return (ICommand)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }
    #endregion
    #endregion
  }
}
