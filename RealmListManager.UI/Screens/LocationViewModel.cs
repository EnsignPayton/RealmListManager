using System.Linq;
using System.Windows;
using Caliburn.Micro;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Events;
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
        private readonly IEventAggregator _eventAggregator;
        private LocationModel _location;

        #endregion

        #region Constructor

        public LocationViewModel(IWindowConductor windowConductor,
            DbConnectionManager connectionManager,
            IEventAggregator eventAggregator)
        {
            _windowConductor = windowConductor;
            _connectionManager = connectionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Properties

        public LocationModel Location
        {
            get => _location;
            set
            {
                _location = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Actions

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
            _eventAggregator.PublishOnUIThread(new LocationChanged { IsDeleted = true, Location = Location });
        }

        /// <summary>
        /// Edits the current location.
        /// </summary>
        public void EditLocation()
        {
            var dialog = _windowConductor.ShowDialog<NewLocationViewModel>(vm => vm.Location = Location.Clone());
            if (dialog.Result == false) return;

            Location = dialog.Location;
            _connectionManager.UpdateLocation(dialog.Location.DataModel);
            _eventAggregator.PublishOnUIThread(new LocationChanged { IsDeleted = false, Location = Location });
        }

        /// <summary>
        /// Opens a dialog to create a new realmlist.
        /// </summary>
        public void AddRealmlist()
        {
            var dialog = _windowConductor.ShowDialog<NewRealmlistViewModel>();

            if (dialog.Result == false) return;
            Location.Realmlists.Add(dialog.NewRealmlist);

            _connectionManager.InsertRealmlist(dialog.NewRealmlist.DataModel, Location.DataModel.Id);
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

        /// <summary>
        /// Starts a location.
        /// </summary>
        /// <param name="realmlist">Optional realmlist to substitute.</param>
        public void Play(RealmlistModel realmlist = null)
        {
            FileUtilities.StartLocation(Location.Path, realmlist?.Url);
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
