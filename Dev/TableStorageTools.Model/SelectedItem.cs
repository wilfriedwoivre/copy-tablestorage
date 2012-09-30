using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableStorageTools.Tools.MVVM;

namespace TableStorageTools.Model
{
    public class SelectedItem : BaseEntity
    {
        private string _value;
        private bool _isSelected;

        public string Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged("Value"); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }
    }
}
