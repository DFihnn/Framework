using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Framework.Threading;

namespace Framework.Windows.Input
{
  /// <summary>
  /// A command whose sole purpose is to 
  /// relay its functionality to other
  /// objects by invoking delegates. The
  /// default return value for the CanExecute
  /// method is 'true'.
  /// </summary>
  public abstract class RelayCommandBase : ICommand
  {
    #region Fields

    private readonly Action<object> executeMethod;
    private readonly Func<object, bool> canExecuteMethod;
    private readonly bool automaticRaiseCanExecuteChanged;
    private List<WeakReference> canExecuteChangedHandlers = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new command that can always executeMethod.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    protected RelayCommandBase(Action<object> executeMethod)
      : this(executeMethod, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    /// <param name="canExecuteMethod">The execution status logic.</param>
    protected RelayCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
      : this(executeMethod, canExecuteMethod, true)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    /// <param name="canExecuteMethod">The execution status logic.</param>
    /// <param name="automaticRaiseCanExecuteChanged"> </param>
    protected RelayCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod, bool automaticRaiseCanExecuteChanged)
    {
      if (executeMethod == null)
        throw new ArgumentNullException("executeMethod");

      this.executeMethod = executeMethod;
      this.canExecuteMethod = canExecuteMethod;
      this.automaticRaiseCanExecuteChanged = canExecuteMethod != null && automaticRaiseCanExecuteChanged;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return canExecuteMethod == null || canExecuteMethod(parameter);
    }

    public void Execute(object parameter)
    {
      executeMethod(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested += CommandManagerOnRequerySuggested;
        WeakEventHandlerManager.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);     
      }
      remove
      {
        WeakEventHandlerManager.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested -= CommandManagerOnRequerySuggested;
      }
    }

    private void CommandManagerOnRequerySuggested(object sender, EventArgs eventArgs)
    {
      RaiseCanExecuteChanged();
    }

    protected virtual void OnCanExecuteChanged()
    {
      WeakEventHandlerManager.CallWeakReferenceHandlers(this, canExecuteChangedHandlers);
    }

    public void RaiseCanExecuteChanged()
    {
      OnCanExecuteChanged();
    }

    #endregion // ICommand Members
  }

  public class RelayCommand : RelayCommandBase
  {
    public RelayCommand(Action executeMethod)
      : this(executeMethod, () => true)
    {
    }

    public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
      : base((o) => executeMethod(), (o) => canExecuteMethod())
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException("executeMethod");
    }

    public void Execute()
    {
      Execute(null);
    }

    public bool CanExecute()
    {
      return CanExecute(null);
    }
  }

  public class RelayCommand<T> : RelayCommandBase
  {
    public RelayCommand(Action<T> executeMethod)
      : this(executeMethod, (o) => true)
    {
    }

    public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
      : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException("executeMethod");

      Type genericType = typeof(T);

      // DelegateCommand allows object or Nullable<>.  
      // note: Nullable<> is a struct so we cannot use a class constraint.
      if (genericType.IsValueType && (!genericType.IsGenericType || !typeof(Nullable<>).IsAssignableFrom(genericType.GetGenericTypeDefinition())))
        throw new InvalidCastException();
    }

    public bool CanExecute(T parameter)
    {
      return base.CanExecute(parameter);
    }

    public void Execute(T parameter)
    {
      base.Execute(parameter);
    }

  }


  public abstract class AsyncRelayCommandBase : ICommand
  {
    #region Fields

    private readonly Func<object, Task> executeMethod;
    private readonly Func<object, bool> canExecuteMethod;
    private readonly bool automaticRaiseCanExecuteChanged;
    private List<WeakReference> canExecuteChangedHandlers = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new command that can always executeMethod.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    protected AsyncRelayCommandBase(Func<object, Task> executeMethod)
      : this(executeMethod, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    /// <param name="canExecuteMethod">The execution status logic.</param>
    protected AsyncRelayCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
      : this(executeMethod, canExecuteMethod, true)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="executeMethod">The execution logic.</param>
    /// <param name="canExecuteMethod">The execution status logic.</param>
    /// <param name="automaticRaiseCanExecuteChanged"> </param>
    protected AsyncRelayCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod, bool automaticRaiseCanExecuteChanged)
    {
      this.executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
      this.canExecuteMethod = canExecuteMethod;
      this.automaticRaiseCanExecuteChanged = canExecuteMethod != null && automaticRaiseCanExecuteChanged;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return canExecuteMethod == null || canExecuteMethod(parameter);
    }

    public async void Execute(object parameter)
    {
      await executeMethod(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        //Enable this code when .net 4.5 are in use
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested += CommandManagerOnRequerySuggested;
        WeakEventHandlerManager.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);
      }
      remove
      {
        //Enable this code when .net 4.5 are in use
        WeakEventHandlerManager.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested -= CommandManagerOnRequerySuggested;
      }
    }

    private void CommandManagerOnRequerySuggested(object sender, EventArgs eventArgs)
    {
      RaiseCanExecuteChanged();
    }

    protected virtual void OnCanExecuteChanged()
    {
      WeakEventHandlerManager.CallWeakReferenceHandlers(this, canExecuteChangedHandlers);
    }

    public void RaiseCanExecuteChanged()
    {
      OnCanExecuteChanged();
    }

    #endregion // ICommand Members
  }


  public class AsyncRelayCommand : AsyncRelayCommandBase
  {
    public AsyncRelayCommand(Func<Task> executeMethod)
      : this(executeMethod, () => true)
    {
    }

    public AsyncRelayCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
      : base(async (o) => await executeMethod(), (o) => canExecuteMethod())
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException(nameof(executeMethod));
    }

  }

  public class AsyncRelayCommand<T> : AsyncRelayCommandBase
  {
    public AsyncRelayCommand(Func<T, Task> executeMethod)
      : this(executeMethod, (o) => true)
    {
    }

    public AsyncRelayCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
      : base(async (o) => await executeMethod((T)o), (o) => canExecuteMethod((T)o))
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException(nameof(executeMethod));

      Type genericType = typeof(T);

      // RelayCommand allows object or Nullable<>.  
      // note: Nullable<> is a struct so we cannot use a class constraint.
      if (genericType.IsValueType && (!genericType.IsGenericType || !typeof(Nullable<>).IsAssignableFrom(genericType.GetGenericTypeDefinition())))
        throw new InvalidCastException();
    }

  }

  public abstract class AsyncAssignmentRelayCommandBase : ICommand
  {
    #region Fields

    protected Func<object, Task> ExecuteMethod { get; set; }
    protected Func<object, bool> CanExecuteMethod { get; set; }
    private readonly bool automaticRaiseCanExecuteChanged = true;
    private List<WeakReference> canExecuteChangedHandlers = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new command that can always executeMethod.
    /// </summary>
    protected AsyncAssignmentRelayCommandBase()
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="automaticRaiseCanExecuteChanged"> </param>
    protected AsyncAssignmentRelayCommandBase(bool automaticRaiseCanExecuteChanged)
    {
      this.automaticRaiseCanExecuteChanged = automaticRaiseCanExecuteChanged;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return CanExecuteMethod == null || CanExecuteMethod(parameter);
    }

    public async void Execute(object parameter)
    {
      try
      {
        if (ExecuteMethod == null)
          return;
        await ExecuteMethod(parameter);
      }
      catch (OperationCanceledException)
      {
      }
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        //Enable this code when .net 4.5 are in use
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested += CommandManagerOnRequerySuggested;
        WeakEventHandlerManager.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);
      }
      remove
      {
        //Enable this code when .net 4.5 are in use
        WeakEventHandlerManager.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
        if (automaticRaiseCanExecuteChanged && (canExecuteChangedHandlers == null || canExecuteChangedHandlers.Count == 0))
          CommandManager.RequerySuggested -= CommandManagerOnRequerySuggested;
      }
    }

    private void CommandManagerOnRequerySuggested(object sender, EventArgs eventArgs)
    {
      RaiseCanExecuteChanged();
    }

    protected virtual void OnCanExecuteChanged()
    {
      WeakEventHandlerManager.CallWeakReferenceHandlers(this, canExecuteChangedHandlers);
    }

    public void RaiseCanExecuteChanged()
    {
      OnCanExecuteChanged();
    }

    #endregion // ICommand Members
  }


  public class AsyncQueueRelayCommand<T> : AsyncAssignmentRelayCommandBase
  {
    private readonly AsyncAwaitQueue<T> asyncAwaitQueue;

    public AsyncQueueRelayCommand(Func<T, CancellationToken, Task> executeMethod)
      : this(executeMethod, (o) => true)
    {
    }

    public AsyncQueueRelayCommand(Func<T, CancellationToken, Task> executeMethod, Func<T, bool> canExecuteMethod)
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException(nameof(executeMethod));

      asyncAwaitQueue = new AsyncAwaitQueue<T>(executeMethod);
      ExecuteMethod = async o => await asyncAwaitQueue.EnqueueAsync((T)o);
      CanExecuteMethod = o => canExecuteMethod((T)o);

      Type genericType = typeof(T);

      // RelayCommand allows object or Nullable<>.  
      // note: Nullable<> is a struct so we cannot use a class constraint.
      if (genericType.IsValueType && (!genericType.IsGenericType || !typeof(Nullable<>).IsAssignableFrom(genericType.GetGenericTypeDefinition())))
        throw new InvalidCastException();
    }
  }
}