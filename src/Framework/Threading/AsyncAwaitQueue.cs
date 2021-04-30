using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Threading
{
  public class AsyncAwaitQueue<T>
  {
    private TaskEntry previousTask = new TaskEntry(Task.CompletedTask, new CancellationTokenSource());
    private Func<T, CancellationToken, Task> func;

    public AsyncAwaitQueue(Func<T, CancellationToken, Task> func)
    {
      this.func = func;
    }

    public async Task EnqueueAsync(T t)
    {
      TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

      TaskEntry previousTaskEntry = Interlocked.Exchange(ref previousTask, new TaskEntry(taskCompletionSource.Task, cancellationTokenSource));

      previousTaskEntry.CancellationTokenSource.Cancel();
      try
      {
        await previousTaskEntry.Task;
      }
      catch (OperationCanceledException)
      {
        // Do nothing;
      }

      try
      {
        cancellationTokenSource.Token.ThrowIfCancellationRequested();
        await func(t, cancellationTokenSource.Token);
        taskCompletionSource.TrySetResult(true);
      }
      catch (OperationCanceledException)
      {
        taskCompletionSource.TrySetCanceled();
      }
      catch (Exception ex)
      {
        taskCompletionSource.TrySetException(ex);
      }
    }

    private class TaskEntry
    {
      public Task Task { get; }

      public CancellationTokenSource CancellationTokenSource { get; }

      public TaskEntry(Task task, CancellationTokenSource cancellationTokenSource)
      {
        Task = task;
        CancellationTokenSource = cancellationTokenSource;
      }
    }
  }
}

