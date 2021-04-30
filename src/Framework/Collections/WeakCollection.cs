using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Collections
{
  public class WeakCollection<T> : IEnumerable<T> where T : class
  {
    private List<WeakReference> innerList = new List<WeakReference>();

    public void Add(T item)
    {
      innerList.Add(new WeakReference(item));
    }

    public void Clear()
    {
      innerList.Clear();
    }

    public bool Remove(T item)
    {
      for (int index = 0; index < innerList.Count; ++index)
      {
        if (innerList[index].Target == item)
        {
          innerList.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public IList<T> ToList()
    {
      List<T> objList = new List<T>(innerList.Count);
      foreach (WeakReference inner in innerList)
      {
        if (inner.Target is T target)
          objList.Add(target);
      }
      if (objList.Count != innerList.Count)
        Prune(objList.Count);
      return objList;
    }

    public int GetAliveItemsCount()
    {
      return innerList.Count(weakRef => weakRef.IsAlive);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ToList().GetEnumerator();
    }

    private void Prune(int anticipatedSize)
    {
      var list = this.innerList;
      innerList = new List<WeakReference>(anticipatedSize);
      foreach (var weakReference in list.Where(weakReference => weakReference.IsAlive))
      {
        innerList.Add(weakReference);
      }
    }
  }
}
