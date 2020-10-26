using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Drawboard.ViewModels
{
    /// <summary>
    /// View base base class. Implements INotifyPropertyChanged for bindable properties.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool Set<T>(ref T target, T value, [CallerMemberName] string propertyName = null)
        {
            // do nothing if nothing has changed
            if (Equals(target, value))
                return false;
            target = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
