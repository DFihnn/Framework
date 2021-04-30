using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Framework.Collections
{
  public class SuspendableObservableCollection<T> : ObservableCollection<T>
  {
    private int suspendChangesCount;
    private bool skippedNotification;

    public IDisposable SuspendChangeNotification()
    {
      return new SuspendChangesScope(this);
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (AreNotificationsSuspended)
        skippedNotification = true;
      else
        base.OnCollectionChanged(e);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (AreNotificationsSuspended)
        skippedNotification = true;
      else
        base.OnPropertyChanged(e);
    }

    private bool AreNotificationsSuspended => suspendChangesCount > 0;

    private class SuspendChangesScope : DisposableObject
    {
      private readonly SuspendableObservableCollection<T> collection;

      public SuspendChangesScope(SuspendableObservableCollection<T> collection)
      {
        this.collection = collection;
        if (this.collection.suspendChangesCount == 0)
          this.collection.skippedNotification = false;
        ++this.collection.suspendChangesCount;
      }

      protected override void DisposeManagedResources()
      {
        --collection.suspendChangesCount;
        if (collection.suspendChangesCount != 0 || !collection.skippedNotification)
          return;
        collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
    }
  }
}
