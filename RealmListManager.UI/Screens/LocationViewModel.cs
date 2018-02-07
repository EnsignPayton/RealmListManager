using System.Linq;
using System.Windows;
using Caliburn.Micro;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Models;
using RealmListManager.UI.Core.Utilities;
using RealmListManager.UI.Dialogs;

namespace RealmListManager.UI.Screens
{
    public class LocationViewModel : Screen
    {
        #region Fields

        private readonly IWindowConductor _windowConductor;
        private readonly DbConnectionManager _connectionManager;

        #endregion

        #region Constructor

        public LocationViewModel(IWindowConductor windowConductor,
            DbConnectionManager connectionManager)
        {
            _windowConductor = windowConductor;
            _connectionManager = connectionManager;
        }

        #endregion

        #region Properties

        public LocationModel Location { get; set; }

        #endregion

        #region Actions

        /// <summary>
        /// Opens a dialog to create a new realmlist.
        /// </summary>
        public void AddRealmlist()
        {
            var newRealmlistDialog = _windowConductor.ShowDialog<NewRealmlistViewModel>();

            if (newRealmlistDialog.Result == false) return;
            Location.Realmlists.Add(newRealmlistDialog.NewRealmlist);

            _connectionManager.InsertRealmlist(newRealmlistDialog.NewRealmlist.DataModel, Location.DataModel.Id);
        }

        /// <summary>
        /// Starts a location.
        /// </summary>
        /// <param name="realmlist">Optional realmlist to substitute.</param>
        public void Play(RealmlistModel realmlist = null)
        {
            FileUtilities.StartLocation(Location.Path, realmlist?.Url);
        }

        /// <summary>
        /// Deletes the current location.
        /// </summary>
        public void DeleteLocation()
        {
            var result = _windowConductor.ShowMessageBox("This location will be permanently deleted. Continue?",
                "Delete Location");
            if (result != MessageBoxResult.OK) return;

            // Delete from the database
            _connectionManager.DeleteLocation(Location.DataModel.Id);

            // Deactivate and dispose this screen
            var shellViewModel = IoC.Get<ShellViewModel>();
            shellViewModel.DeleteLocation(Location);
        }

        /// <summary>
        /// Deletes a realmlist.
        /// </summary>
        /// <param name="realmlist">Realmlist</param>
        public void DeleteRealmlist(RealmlistModel realmlist)
        {
            var result = _windowConductor.ShowMessageBox("This realmlist will be permanently deleted. Continue?",
                "Delete Realmlist");
            if (result != MessageBoxResult.OK) return;

            // Delete from the database
            _connectionManager.DeleteRealmlist(realmlist.DataModel.Id);

            Location.Realmlists.Remove(realmlist);
        }

        #endregion

        #region Methods

        protected override void OnInitialize()
        {
            var savedRealmlists = _connectionManager.QueryRealmlistsByLocation(Location.DataModel.Id);
            foreach (var realmlist in savedRealmlists)
            {
                if (Location.Realmlists.All(x => x.DataModel.Id != realmlist.Id))
                    Location.Realmlists.Add(new RealmlistModel(realmlist));
            }
            base.OnInitialize();
        }

        #endregion
    }
}
