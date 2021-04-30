using System;
using System.Runtime.InteropServices;

namespace Framework
{
  [ComVisible(true)]
  public class DisposableObject : IDisposable
  {
    private EventHandler disposing;

    ~DisposableObject()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public bool IsDisposed { get; private set; }

    public event EventHandler Disposing
    {
      add
      {
        ThrowIfDisposed();
        disposing += value;
      }
      remove => disposing -= value;
    }

    protected void ThrowIfDisposed()
    {
      if (IsDisposed)
        throw new ObjectDisposedException(GetType().Name);
    }

    protected void Dispose(bool isDisposing)
    {
      if (IsDisposed)
        return;
      try
      {
        if (isDisposing)
        {
          disposing?.Invoke(this, new EventArgs());
          disposing = null;
          DisposeManagedResources();
        }
        DisposeNativeResources();
      }
      finally
      {
        IsDisposed = true;
      }
    }

    protected virtual void DisposeManagedResources()
    {
    }

    protected virtual void DisposeNativeResources()
    {
    }
  }
}
