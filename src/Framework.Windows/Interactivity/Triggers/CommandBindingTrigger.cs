using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Triggers
{
  public sealed class CommandBindingTrigger : TriggerBase<FrameworkElement>
  {
    private CommandBinding commandBinding;

    #region Command
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        "Command", typeof(ICommand), typeof(CommandBindingTrigger), new PropertyMetadata(OnCommandChanged));

    public ICommand Command
    {
      get { return (ICommand)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }
    
    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((CommandBindingTrigger)d).TryAddCommandBinding();
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
      
      commandBinding = new CommandBinding(Command, OnCommandExecuted);
      AssociatedObject.CommandBindings.Add(commandBinding);
    }
    
    private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      InvokeActions(null);
    }
  }
}

