using System;
using System.Reactive;
using System.Windows;
using Framework.Extensions;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Triggers
{
  public class ObservableTrigger : TriggerBase<FrameworkElement>
  {
    private IDisposable subscription;

    #region Observable

    /// <summary>
    /// Observable Dependency Property
    /// </summary>
    public static readonly DependencyProperty ObservableProperty =
        DependencyProperty.Register("Observable", typeof(IObservable<Unit>), typeof(ObservableTrigger),
            new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(OnObservableChanged)));

    /// <summary>
    /// Gets or sets the Observable property.  This dependency property 
    /// indicates ....
    /// </summary>
    public IObservable<Unit> Observable
    {
      get { return (IObservable<Unit>)GetValue(ObservableProperty); }
      set { SetValue(ObservableProperty, value); }
    }

    /// <summary>
    /// Handles changes to the Observable property.
    /// </summary>
    private static void OnObservableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ObservableTrigger)d).TryAddSubscription();
    }

    #endregion

    protected override void OnAttached()
    {
      base.OnAttached();
      TryAddSubscription();
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();

      if (subscription != null)
        subscription.Dispose();

      subscription = null;
    }


    private void TryAddSubscription()
    {
      if (AssociatedObject == null || Observable == null)
        return;

      if (subscription != null)
        subscription.Dispose();

      subscription = Observable.SubscribeWeakly(this, OnSubscribeWeakly);
    }

    private static void OnSubscribeWeakly(ObservableTrigger context, Unit args)
    {
      context.InvokeActions(null);
    }
  }
}
