using System.Linq;
using Caliburn.Micro;
using RealmListManager.UI.Core.Models;
using RealmListManager.UI.Core.Utilities;
using RealmListManager.UI.Dialogs;

namespace RealmListManager.UI.Location
{
    public class LocationViewModel : Screen
    {
        #region Fields

        private readonly IWindowManager _windowManager;
        private readonly DbConnectionManager _connectionManager;
        private RealmlistModel _selectedRealmlist;

        #endregion

        #region Constructor

        public LocationViewModel(IWindowManager windowManager,
            DbConnectionManager connectionManager)
        {
            _windowManager = windowManager;
            _connectionManager = connectionManager;
        }

        #endregion

        #region Properties

        public LocationModel Location { get; set; }

        public RealmlistModel SelectedRealmlist
        {
            get => _selectedRealmlist;
            set
            {
                if (_selectedRealmlist == value) return;
                _selectedRealmlist = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Opens a dialog to create a new realmlist.
        /// </summary>
        public void AddRealmlist()
        {
            var newRealmlistViewModel = IoC.Get<NewRealmlistViewModel>();
            _windowManager.ShowDialog(newRealmlistViewModel);

            if (newRealmlistViewModel.Result == false) return;
            Location.Realmlists.Add(newRealmlistViewModel.NewRealmlist);

            _connectionManager.InsertRealmlist(newRealmlistViewModel.NewRealmlist.DataModel, Location.DataModel.Id);

            if (Location.Realmlists.Count == 1)
                SelectedRealmlist = Location.Realmlists.First();
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
