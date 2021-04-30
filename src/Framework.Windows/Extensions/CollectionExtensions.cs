using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Framework.Windows.Extensions
{
  public static class CollectionExtensions
  {

    public static void SetCustomSort<T>(this ObservableCollection<T> collection, IComparer comparer)
    {
      if (CollectionViewSource.GetDefaultView(collection) is ListCollectionView collectionView)
        collectionView.CustomSort = comparer;
    }

    public static object EnableCollectionSynchronization<T>(this ObservableCollection<T> collection)
    {
      var syncLock = ((ICollection)collection).SyncRoot;
      BindingOperations.EnableCollectionSynchronization(collection, syncLock);
      return syncLock;
    }
  }
}
