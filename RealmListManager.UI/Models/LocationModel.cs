using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealmListManager.UI.Models
{
    public class LocationModel : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _path;
        private string _imagePath;
        private ObservableCollection<RealmlistModel> _realmlists;

        #endregion

        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                if (_path == value) return;
                _path = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath == value) return;
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RealmlistModel> Realmlists
        {
            get => _realmlists;
            set
            {
                if (_realmlists == value) return;
                _realmlists = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
