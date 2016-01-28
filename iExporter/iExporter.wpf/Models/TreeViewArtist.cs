namespace iExporter.wpf.Models
{
    public class TreeViewArtist : BaseClass
    {
        private bool _isSelected;
        //private bool _isEnabled;
        private string _name;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        //public bool IsEnabled
        //{
        //    get { return _isEnabled; }
        //    set
        //    {
        //        if (value == _isEnabled) return;
        //        _isEnabled = value;
        //        OnPropertyChanged();
        //    }
        //}

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}