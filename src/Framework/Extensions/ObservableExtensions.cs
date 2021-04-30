using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Extensions
{
  public static class ObservableExtensions
  {
    public static IDisposable SubscribeWeakly<T, TTarget>(this IObservable<T> observable, TTarget target, Action<TTarget, T> onNext) where TTarget : class
    {
      var reference = new WeakReference(target);

      if (onNext.Target != null)
      {
        throw new ArgumentException("onNext must refer to a static method, or else the subscription will still hold a strong reference to target");
      }

      IDisposable subscription = null;
      subscription = observable.Subscribe(item =>
      {
        var currentTarget = reference.Target as TTarget;
        if (currentTarget != null)
        {
          onNext(currentTarget, item);
        }
        else
        {
          if (subscription != null) subscription.Dispose();
        }
      });

      return subscription;
    }

    public static IDisposable SubscribeSafe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
    {
      Contract.Requires(source != null);
      Contract.Requires(onNext != null);
      Contract.Requires(onError != null);
      Contract.Requires(onCompleted != null);
      Contract.Ensures(Contract.Result<IDisposable>() != null);

      return source.SubscribeSafe(Observer.Create<T>(onNext, onError, onCompleted));
    }

    /// <summary>
    /// Generates a sequence of lists where each list contains all values that were observed from 
    /// the <paramref name="source"/> while the previous list was being observed.
    /// </summary>
    /// <typeparam name="TSource">The object that provides notification information.</typeparam>
    /// <param name="source">The observable sequence from which to create introspection lists.</param>
    /// <param name="scheduler">Schedules when lists are observed.</param>
    /// <remarks>
    /// <para>
    /// The lists in the returned observable are never empty. They will always contain at least one element.
    /// </para>
    /// </remarks>
    /// <returns>The source observable sequence buffered into introspection lists.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
      Justification = "All disposables are composited by the subject that is returned to the caller.")]
    public static IObservable<IList<TSource>> BufferIntrospective<TSource>(
      this IObservable<TSource> source, IScheduler scheduler)
    {
      Contract.Requires(source != null);
      Contract.Requires(scheduler != null);
      Contract.Ensures(Contract.Result<IObservable<IList<TSource>>>() != null);

      return Observable.Create<IList<TSource>>(
        observer =>
        {
          var queue = new Queue<TSource>();

          var pendingDrain = false;
          var sourceCompleted = false;
          var gate = new object();

          var sourceSubscription = new SingleAssignmentDisposable();
          var drainSchedule = new SerialDisposable();
          var schedules = new CompositeDisposable(drainSchedule);

          sourceSubscription.Disposable = source.SubscribeSafe(
            value =>
            {
              lock (gate)
              {
                queue.Enqueue(value);

                if (pendingDrain)
                {
                  return;
                }

                pendingDrain = true;
              }

              drainSchedule.Disposable =
                scheduler.Schedule(self =>
                {
                  IList<TSource> buffer;

                  lock (gate)
                  {
                    Contract.Assume(queue.Count > 0);

                    buffer = queue.ToList().AsReadOnly();

                    queue.Clear();
                  }

                  Contract.Assert(buffer.Count > 0);

                  observer.OnNext(buffer);

                  Contract.Assume(pendingDrain);

                  bool loop, completeNow;

                  lock (gate)
                  {
                    pendingDrain = queue.Count > 0;
                    loop = pendingDrain;
                    completeNow = !pendingDrain && sourceCompleted;
                  }

                  if (completeNow)
                  {
                    Contract.Assume(!loop);
                    Contract.Assume(!pendingDrain);

                    observer.OnCompleted();
                  }
                  else if (loop)
                  {
                    self();
                  }
                });
            },
            ex => schedules.Add(scheduler.Schedule(ex,
              (_, error) =>
              {
                observer.OnError(error);
                return Disposable.Empty;
              })),
            () =>
            {
              var completeNow = false;

              lock (gate)
              {
                sourceCompleted = true;
                completeNow = !pendingDrain;
              }

              if (completeNow)
              {
                // Scheduling the call to OnCompleted matches the ObserveOn operator's behavior.
                schedules.Add(scheduler.Schedule(observer.OnCompleted));
              }
            });

          return new CompositeDisposable(sourceSubscription, drainSchedule, schedules);
        });
    }

    public static Task<List<T>> ToListAsync<T>(this IObservable<T> source)
    {
      var taskCompletionSource = new TaskCompletionSource<List<T>>();
      var list = new List<T>();
      source.Subscribe(obj => list.Add(obj), exception => taskCompletionSource.SetException(exception), () => taskCompletionSource.SetResult(list));
      return taskCompletionSource.Task;
    }

    public static Task<List<T>> ToListAsync<T>(this IObservable<T> source, CancellationToken cancellationToken)
    {
      var taskCompletionSource = new TaskCompletionSource<List<T>>();
      var list = new List<T>();
      var disposable = source.Subscribe(obj => list.Add(obj), exception => taskCompletionSource.SetException(exception), () => taskCompletionSource.SetResult(list));
      cancellationToken.Register(() => disposable?.Dispose());
      return taskCompletionSource.Task;
    }

    public static Task CompletionTask<T>(this IObservable<T> source)
    {
      var taskCompletionSource = new TaskCompletionSource<bool>();
      source.Subscribe(obj => { }, exception => taskCompletionSource.SetException(exception), () => taskCompletionSource.SetResult(true));
      return taskCompletionSource.Task;
    }

    public static Task CompletionTask<T>(this IObservable<T> source, CancellationToken cancellationToken)
    {
      var taskCompletionSource = new TaskCompletionSource<bool>();
      var disposable = source.Subscribe(obj => { }, exception => taskCompletionSource.SetException(exception), () => taskCompletionSource.SetResult(true));
      cancellationToken.Register(() => disposable?.Dispose());
      return taskCompletionSource.Task;
    }

    public static IObservable<IList<T>> FirstThenBufferUntilInactive<T>(this IObservable<T> source, TimeSpan delay)
    {
      var closes = source.Throttle(delay);
      return source.Window(() => closes).SelectMany(window => window.Take(1).Select(t => new List<T> { t }).Concat(window.ToList()));
    }

    public static IObservable<Unit> ToUnit<T>(this IObservable<T> source)
    {
      return source.Select(x => Unit.Default);
    }


  }

}
