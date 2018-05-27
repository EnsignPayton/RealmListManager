using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Events;
using RealmListManager.UI.Core.Models;
using RealmListManager.UI.Dialogs;

namespace RealmListManager.UI.Screens
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive,
        IWindowConductor, IHandle<LocationChanged>, IHandle<OptionsClosed>
    {
        #region Fields

        private readonly IWindowManager _windowManager;
        private readonly DbConnectionManager _connectionManager;
        private readonly IConfigurationManager _configurationManager;

        private ObservableCollection<LocationModel> _locations;
        private LocationModel _selectedLocation;

        #endregion

        #region Constructor

        public ShellViewModel(IWindowManager windowManager,
            DbConnectionManager connectionManager,
            IConfigurationManager configurationManager,
            IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _connectionManager = connectionManager;
            _configurationManager = configurationManager;
            eventAggregator.Subscribe(this);

            // Populate locations from database
            var savedLocations = _connectionManager.QueryLocations().OrderBy(x => x.Index);
            Locations = new ObservableCollection<LocationModel>(savedLocations.Select(x => new LocationModel(x)));
        }

        #endregion

        #region Properties

        public ObservableCollection<LocationModel> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                NotifyOfPropertyChange();
            }
        }

        public LocationModel SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;

                if (_selectedLocation != null)
                    Show<LocationViewModel>(vm => vm.Location = _selectedLocation);

                NotifyOfPropertyChange();
            }
        }

        public bool MinimizeToTray => _configurationManager.MinimizeToTray;

        #endregion

        #region IWindowConductor

        public void Show<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            ActivateItem(screen);
        }

        public T ShowDialog<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowDialog(screen);
            return screen;
        }

        public T ShowWindow<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowWindow(screen);
            return screen;
        }

        public T ShowPopup<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowPopup(screen);
            return screen;
        }

        public MessageBoxResult ShowMessageBox(string message, string title = null,
            MessageBoxButton buttonType = MessageBoxButton.OKCancel)
        {
            var messageBox = ShowDialog<MessageBoxViewModel>(vm =>
            {
                vm.Title = title;
                vm.Message = message;
                vm.ButtonType = buttonType;
            });

            return messageBox.Result;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Opens a dialog to add a new location.
        /// </summary>
        public void AddLocation()
        {
            var dialog = ShowDialog<NewLocationViewModel>();
            if (dialog.Result == false) return;
            Locations.Add(dialog.Location);
            dialog.Location.Index = Locations.IndexOf(dialog.Location);

            _connectionManager.InsertLocation(dialog.Location.DataModel);

            if (Locations.Count == 1)
                SelectedLocation = Locations.First();
        }

        /// <summary>
        /// Deletes a location and cleans up the UI.
        /// </summary>
        /// <param name="location">Location</param>
        public void DeleteLocation(LocationModel location)
        {
            // Delete from the database
            _connectionManager.DeleteLocation(location.DataModel.Id);
            Locations.Remove(location);

            // Update indices
            for (int i = 0; i < Locations.Count; i++)
            {
                Locations[i].Index = i;
                _connectionManager.UpdateLocation(Locations[i].DataModel);
            }

            if (Locations.Count > 1)
            {
                SelectedLocation = Locations.First(x => x != location);
            }
            else
            {
                SelectedLocation = null;
                Show<FirstTimeViewModel>();
            }
        }

        public void ShowOptions()
        {
            Show<OptionsViewModel>();
        }

        #endregion

        #region Methods

        protected override void OnViewAttached(object view, object context)
        {
            if (Locations.Any()) SelectedLocation = Locations.First();
            if (ActiveItem == null) Show<FirstTimeViewModel>();
            base.OnViewAttached(view, context);
        }

        protected override void OnDeactivate(bool close)
        {
            // Update indices to reflect drag/drop
            for (int i = 0; i < Locations.Count; i++)
            {
                Locations[i].Index = i;
                _connectionManager.UpdateLocation(Locations[i].DataModel);
            }

            base.OnDeactivate(close);
        }

        public void Handle(LocationChanged message)
        {
            if (message.IsDeleted)
            {
                DeleteLocation(message.Location);
            }
            else
            {
                var selectedId = message.Location.DataModel.Id;
                var savedLocations = _connectionManager.QueryLocations().OrderBy(x => x.Index);
                Locations = new ObservableCollection<LocationModel>(savedLocations.Select(x => new LocationModel(x)));
                SelectedLocation = Locations.FirstOrDefault(x => x.DataModel.Id == selectedId);
            }
        }

        public void Handle(OptionsClosed message)
        {
            if (SelectedLocation != null)
                SelectedLocation = SelectedLocation;
            else if (Locations.Any())
                SelectedLocation = Locations.First();
            else
                Show<FirstTimeViewModel>();
        }

        #endregion
    }
}
