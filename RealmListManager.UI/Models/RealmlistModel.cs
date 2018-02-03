using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealmListManager.UI.Models
{
    public class RealmlistModel : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _url;
        private string _imagePath;

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

        public string Url
        {
            get => _url;
            set
            {
                if (_url == value) return;
                _url = value;
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
