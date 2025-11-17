using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GWG.UsoUIElements.Templates
{
    /// <summary>
    /// The UsoDataTemplate class serves as an example base for defining bindable data objects.
    /// It inherits from the ScriptableObject class and implements the INotifyPropertyChanged interface,
    /// allowing for property change notification and data-binding functionality.
    /// </summary>
    public class UsoDataTemplate : ScriptableObject, INotifyPropertyChanged
    {
        // ////////////////////////////////////////////////////////////////
        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}