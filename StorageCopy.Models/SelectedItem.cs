namespace StorageCopy.Models
{
    public class SelectedItem : BaseEntity
    {
        private string _value;
        private bool _isSelected;

        public string Value
        {
            get => _value;
            set { _value = value; RaisePropertyChanged("Value"); }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }
    }
}