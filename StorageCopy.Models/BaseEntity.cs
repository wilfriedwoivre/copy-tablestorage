using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace StorageCopy.Models
{
    public abstract class BaseEntity : INotifyPropertyChanged
    {

        public virtual void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
