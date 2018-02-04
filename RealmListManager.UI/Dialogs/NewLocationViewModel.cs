using Caliburn.Micro;
using RealmListManager.UI.Models;

namespace RealmListManager.UI.Dialogs
{
    public class NewLocationViewModel : Screen
    {
        #region Constructor

        public NewLocationViewModel()
        {
            NewLocation = new LocationModel();
            NewLocation.PropertyChanged += NewLocation_PropertyChanged;
        }

        #endregion

        #region Properties

        public LocationModel NewLocation { get; }

        public bool Result { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(NewLocation.Name) &&
                               !string.IsNullOrWhiteSpace(NewLocation.Path);

        #endregion

        #region Actions

        public void Save()
        {
            Result = true;
            TryClose();
        }

        public void Cancel()
        {
            Result = false;
            TryClose();
        }

        #endregion

        #region Events

        private void NewLocation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanSave);
        }

        #endregion
    }
}
