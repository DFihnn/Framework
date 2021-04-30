using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Framework.Extensions
{
  public static class NotifyPropertyChangedExtensions
  {
    public static IObservable<EventPattern<PropertyChangedEventArgs>> PropertyChangedObservable(this INotifyPropertyChanged instance)
    {
      return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
          handler => (sender, e) => handler(sender, e),
          handler => instance.PropertyChanged += handler,
          handler => instance.PropertyChanged -= handler);
    }
  }
}
