using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Framework.Collections
{
  public class ObservableNotifiableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
  {
    public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

    protected override void ClearItems()
    {
      foreach (var item in Items)
        item.PropertyChanged -= OnItemPropertyChanged;

      base.ClearItems();
    }

    protected override void InsertItem(int index, T item)
    {
      if (item != null)
      {
        base.InsertItem(index, item);
        item.PropertyChanged += OnItemPropertyChanged;
      }
    }

    protected override void RemoveItem(int index)
    {
      var item = this[index];
      if (item != null)
        item.PropertyChanged -= OnItemPropertyChanged;
      base.RemoveItem(index);
    }

    protected override void SetItem(int index, T item)
    {
      var oldItem = this[index];
      if (oldItem != null)
        oldItem.PropertyChanged -= OnItemPropertyChanged;
      if (item != null)
        item.PropertyChanged += OnItemPropertyChanged;
      base.SetItem(index, item);
    }

    void OnItemPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
      ItemPropertyChanged?.Invoke(this, new ItemPropertyChangedEventArgs(sender, args.PropertyName));
    }
  }

  public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
  {
    public ItemPropertyChangedEventArgs(object item, string propertyName) : base(propertyName)
    {
      Item = item;
    }

    public object Item { get; }
  }
}
