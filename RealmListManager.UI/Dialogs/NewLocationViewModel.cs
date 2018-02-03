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
        }

        #endregion

        #region Properties

        public LocationModel NewLocation { get; }

        public bool Result { get; private set; }

        #endregion

        #region Actions

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(NewLocation.Name) ||
                string.IsNullOrWhiteSpace(NewLocation.Path))
            {
                // Must have those two defined
                return;
            }

            Result = true;
            TryClose();
        }

        public void Cancel()
        {
            Result = false;
            TryClose();
        }

        #endregion
    }
}
