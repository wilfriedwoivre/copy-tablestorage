using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCopy.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public abstract void OnLoad();
        public abstract void OnUnload();

        public virtual void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
