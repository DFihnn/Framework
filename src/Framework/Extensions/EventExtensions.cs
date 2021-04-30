using System;

namespace Framework.Extensions
{
  public static class EventExtensions
  {
    public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object source, TEventArgs args) where TEventArgs : EventArgs
    {
      eventHandler?.Invoke(source, args);
    }

    public static void RaiseEvent(this EventHandler eventHandler, object source)
    {
      eventHandler.RaiseEvent(source, EventArgs.Empty);
    }

    public static void RaiseEvent(this EventHandler eventHandler, object source, EventArgs args)
    {
      eventHandler?.Invoke(source, args);
    }
  }
}
