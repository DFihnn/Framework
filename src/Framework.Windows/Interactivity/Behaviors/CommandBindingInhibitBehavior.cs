using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public sealed class CommandBindingInhibitBehavior : Behavior<FrameworkElement>
  {
    private CommandBinding commandBinding;

    #region Command

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        "Command", typeof(ICommand), typeof(CommandBindingInhibitBehavior), new PropertyMetadata(OnCommandChanged));

    public ICommand Command
    {
      get { return (ICommand)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((CommandBindingInhibitBehavior)d).TryAddCommandBinding();
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
      {
        AssociatedObject.CommandBindings.Remove(commandBinding);
        commandBinding.PreviewCanExecute -= PreviewCanExecute;
      }

      commandBinding = new CommandBinding(Command);
      commandBinding.PreviewCanExecute += PreviewCanExecute;
      AssociatedObject.CommandBindings.Add(commandBinding);
    }

    private void PreviewCanExecute(object sender, CanExecuteRoutedEventArgs args)
    {
      args.ContinueRouting = false;
      args.CanExecute = false;
    }
  }
}