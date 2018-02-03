using System.Collections.ObjectModel;
using Caliburn.Micro;
using RealmListManager.UI.Dialogs;
using RealmListManager.UI.Models;

namespace RealmListManager.UI.Shell
{
    public class ShellViewModel : Conductor<IScreen>
    {
        #region Fields

        private IWindowManager _windowManager;

        #endregion

        #region Constructor

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            Locations = new ObservableCollection<LocationModel>();
        }

        #endregion

        #region Properties

        public ObservableCollection<LocationModel> Locations { get; set; }

        #endregion

        #region Actions

        public void AddLocation()
        {
            var newLocationViewModel = IoC.Get<NewLocationViewModel>();
            _windowManager.ShowDialog(newLocationViewModel);

            if (newLocationViewModel.Result == false) return;
            Locations.Add(newLocationViewModel.NewLocation);
        }

        #endregion
    }
}
