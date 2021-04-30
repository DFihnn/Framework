using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public sealed class CommandBindingRelayBehavior : Behavior<FrameworkElement>
  {
    private CommandBinding commandBinding;

    #region Command
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        "Command", typeof(ICommand), typeof(CommandBindingRelayBehavior), new PropertyMetadata(OnCommandChanged));

    public ICommand Command
    {
      get { return (ICommand)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((CommandBindingRelayBehavior)d).TryAddCommandBinding();
    }

    #endregion

    #region CommandTo

    public static readonly DependencyProperty CommandToProperty = DependencyProperty.Register(
        "CommandTo", typeof(ICommand), typeof(CommandBindingRelayBehavior));

    public ICommand CommandTo
    {
      get { return (ICommand)GetValue(CommandToProperty); }
      set { SetValue(CommandToProperty, value); }
    }

    #endregion

    #region CommandParameter

    /// <summary>
    /// CommandParameter Dependency Property
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandBindingRelayBehavior),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.None));

    /// <summary>
    /// Gets or sets the CommandParameter property.  This dependency property 
    /// indicates ....
    /// </summary>
    public object CommandParameter
    {
      get { return GetValue(CommandParameterProperty); }
      set { SetValue(CommandParameterProperty, value); }
    }

    #endregion

    protected override void OnAttached()
    {
      base.OnAttached();
      TryAddCommandBinding();
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();

      if (commandBinding != null)
        AssociatedObject.CommandBindings.Remove(commandBinding);

      commandBinding = null;
    }
    
    private void TryAddCommandBinding()
    {
      if (AssociatedObject == null)
        return;

      if (commandBinding != null)
        AssociatedObject.CommandBindings.Remove(commandBinding);

      commandBinding = new CommandBinding(Command, OnCommandExecuted, OnCommandCanExecute);
      AssociatedObject.CommandBindings.Add(commandBinding);
    }

    private void OnCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      var pararmeter = CommandParameter ?? e.Parameter;
      if (CommandTo != null)
        e.CanExecute = CommandTo.CanExecute(pararmeter);
    }

    private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      var pararmeter = CommandParameter ?? e.Parameter;
      CommandTo.Execute(pararmeter);
    }
  }
}