using System;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace Framework.Extensions
{
  public static class NotifyCollectionChangedExtensions
  {
    public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> NotifyCollectionChangedObservable(this INotifyCollectionChanged instance)
    {
      return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
          handler => (sender, e) => handler(sender, e),
          handler => instance.CollectionChanged += handler,
          handler => instance.CollectionChanged -= handler);
    }
  }
}
