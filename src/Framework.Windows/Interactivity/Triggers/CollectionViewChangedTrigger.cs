using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using Framework.Extensions;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Triggers
{
  public class CollectionViewChangedTrigger : TriggerBase<FrameworkElement>
  {
    private IDisposable collectionChangedSubscription;

    #region CollectionView

    /// <summary>
    /// CollectionView Dependency Property
    /// </summary>
    public static readonly DependencyProperty CollectionViewProperty =
        DependencyProperty.Register("CollectionView", typeof(ICollectionView), typeof(CollectionViewChangedTrigger),
            new FrameworkPropertyMetadata(null,
                new PropertyChangedCallback(OnCollectionViewChanged)));

    /// <summary>
    /// Gets or sets the CollectionView property.  This dependency property 
    /// indicates ....
    /// </summary>
    public ICollectionView CollectionView
    {
      get { return (ICollectionView)GetValue(CollectionViewProperty); }
      set { SetValue(CollectionViewProperty, value); }
    }

    /// <summary>
    /// Handles changes to the CollectionView property.
    /// </summary>
    private static void OnCollectionViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((CollectionViewChangedTrigger)d).OnCollectionViewChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the CollectionView property.
    /// </summary>
    protected virtual void OnCollectionViewChanged(DependencyPropertyChangedEventArgs e)
    {
      if (collectionChangedSubscription != null)
        collectionChangedSubscription.Dispose();

      var newCollectionView = e.NewValue as ICollectionView;
      if (newCollectionView != null)
      {
        var canExecuteEvent = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                                                          ev => newCollectionView.CollectionChanged += ev,
                                                          ev => newCollectionView.CollectionChanged -= ev);
        collectionChangedSubscription = canExecuteEvent.SubscribeWeakly(this, OnSubscribeWeakly);
      }
    }

    private static void OnSubscribeWeakly(CollectionViewChangedTrigger context, EventPattern<NotifyCollectionChangedEventArgs> args)
    {
      context.CollectionChanged(args.Sender, args.EventArgs);
    }


    void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      InvokeActions(null);
    }

    #endregion

    protected override void OnDetaching()
    {
      base.OnDetaching();

      collectionChangedSubscription.Dispose();
    }

  }
}
