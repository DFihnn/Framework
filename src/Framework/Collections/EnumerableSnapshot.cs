using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Framework.Collections
{
  public sealed class EnumerableSnapshot : DisposableObject, IEnumerable
  {
    private readonly IList stableList;
    private INotifyCollectionChanged changeSource;

    public EnumerableSnapshot(IEnumerable source)
    {
      stableList = new List<object>(source.Cast<object>());
      ChangeSource = source as INotifyCollectionChanged;
    }

    public bool DetectedChange { get; private set; }

    private INotifyCollectionChanged ChangeSource
    {
      set
      {
        if (changeSource == value)
          return;
        if (changeSource != null)
          changeSource.CollectionChanged -= OnCollectionChanged;
        changeSource = value;
        if (changeSource == null)
          return;
        changeSource.CollectionChanged += OnCollectionChanged;
      }
    }

    private void OnCollectionChanged(object sender, EventArgs e)
    {
      DetectedChange = true;
      ChangeSource = null;
    }

    protected override void DisposeManagedResources()
    {
      ChangeSource = null;
    }

    public IEnumerator GetEnumerator()
    {
      return stableList.GetEnumerator();
    }
  }
}
