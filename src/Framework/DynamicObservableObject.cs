using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using Framework.Utilities;

namespace Framework
{
  public abstract class DynamicObservableObject : DynamicObject, INotifyPropertyChanged
  {
    protected Dictionary<string, object> Properties = new Dictionary<string, object>();

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = null;
      lock (((ICollection)Properties).SyncRoot)
      {
        Properties.TryGetValue(string.Intern(binder.Name), out result);
      }
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      lock (((ICollection)Properties).SyncRoot)
      {
        Properties[string.Intern(binder.Name)] = value;
      }
      OnPropertyChanged(binder.Name);
      return true;
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
      lock (((ICollection)Properties).SyncRoot)
      {
        return Properties.Keys.ToList();
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Internal data is of type ConcurrentDictionary<string,object>")]
    public virtual bool TryGetProperty(string propertyName, out object result)
    {
      lock (((ICollection)Properties).SyncRoot)
      {
        return Properties.TryGetValue(string.Intern(propertyName), out result);
      }
    }

    public virtual void SetProperty(string propertyName, object value)
    {
      var localPropertyName = string.Intern(propertyName);
      lock (((ICollection)Properties).SyncRoot)
      {
        Properties[localPropertyName] = value;
      }
      OnPropertyChanged(propertyName);
    }

    public IEnumerable<KeyValuePair<string, object>> KeyValuePairs()
    {
      lock (((ICollection)Properties).SyncRoot)
      {
        return Properties;
      }
    }

    public T GetPropertyValueAs<T>(string propName)
    {
      return Reflection.GetPropertyValueAs<T>(this, propName);
    }

    public bool TryGetPropertyValueAs<T>(string propName, out T result)
    {
      return Reflection.TryGetPropertyValueAs<T>(this, propName, out result);
    }

    #region INotifyPropertyChanged
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="storage">Reference to a property with both getter and setter.</param>
    /// <param name="value">Desired value for the property.</param>
    /// <param name="propertyName">Name of the property used to notify listeners. In .net 4.5 this
    /// value is optional and can be provided automatically when invoked from compilers that
    /// support CallerMemberName.</param>
    /// <returns>True if the value was changed, false if the existing value matched the
    /// desired value.</returns> 
    [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
      if (Equals(storage, value)) return false;
      storage = value;
      OnPropertyChanged(propertyName);
      return true;
    }

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">Name of the property used to notify listeners.</param>    
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Forcefully removes all listeners from PropertyChanged event. Think carefully before use!
    /// </summary>
    /// <remarks>
    /// Think carefully before use!
    /// </remarks>
    protected void ForcefullyRemovePropertyChangedHandlers()
    {
      var eventHandler = PropertyChanged;
      if (eventHandler != null)
      {
        foreach (var @delegate in eventHandler.GetInvocationList())
        {
          var handler = (PropertyChangedEventHandler)@delegate;
          PropertyChanged -= handler;
        }
      }
    }

    #endregion

  }
}