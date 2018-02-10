using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Models;
using RealmListManager.UI.Core.Utilities;
using RealmListManager.UI.Dialogs;

namespace RealmListManager.UI.Screens
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IWindowConductor
    {
        #region Fields

        private readonly IWindowManager _windowManager;
        private readonly DbConnectionManager _connectionManager;

        private LocationModel _selectedLocation;

        #endregion

        #region Constructor

        public ShellViewModel(IWindowManager windowManager,
            DbConnectionManager connectionManager)
        {
            _windowManager = windowManager;
            _connectionManager = connectionManager;

            // Populate locations from database
            var savedLocations = _connectionManager.QueryLocations();
            Locations = new ObservableCollection<LocationModel>(savedLocations.Select(x => new LocationModel(x)));
        }

        #endregion

        #region Properties

        public ObservableCollection<LocationModel> Locations { get; set; }

        public LocationModel SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_selectedLocation == value) return;
                _selectedLocation = value;

                if (_selectedLocation != null)
                    Show<LocationViewModel>(vm => vm.Location = _selectedLocation);

                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Opens a dialog to add a new location.
        /// </summary>
        public void AddLocation()
        {
            var newLocationViewModel = ShowDialog<NewLocationViewModel>();
            if (newLocationViewModel.Result == false) return;
            Locations.Add(newLocationViewModel.NewLocation);

            _connectionManager.InsertLocation(newLocationViewModel.NewLocation.DataModel);

            if (Locations.Count == 1)
                SelectedLocation = Locations.First();
        }

        /// <summary>
        /// Deletes a location and cleans up the UI.
        /// </summary>
        /// <param name="location">Location</param>
        public void DeleteLocation(LocationModel location)
        {
            if (Locations.Count > 1)
            {
                SelectedLocation = Locations.First(x => x != location);
            }
            else
            {
                SelectedLocation = null;
                Show<FirstTimeViewModel>();
            }

            Locations.Remove(location);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show the appropriate screen on startup.
        /// </summary>
        protected override void OnViewAttached(object view, object context)
        {
            if (Locations.Any()) SelectedLocation = Locations.First();
            if (ActiveItem == null) Show<FirstTimeViewModel>();
            base.OnViewAttached(view, context);
        }

        /// <summary>
        /// Show a screen as the ActiveItem.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        public void Show<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            ActivateItem(screen);
        }

        /// <summary>
        /// Show a screen in a modal dialog.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        public T ShowDialog<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowDialog(screen);
            return screen;
        }

        /// <summary>
        /// Show a screen in a non-modal window.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        public T ShowWindow<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowWindow(screen);
            return screen;
        }

        /// <summary>
        /// Show a screen in a popup control at the current mouse position.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        public T ShowPopup<T>(Action<T> initAction = null) where T : IScreen
        {
            var screen = IoC.Get<T>();
            initAction?.Invoke(screen);
            _windowManager.ShowPopup(screen);
            return screen;
        }

        /// <summary>
        /// Show a message box in the application's theme.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title</param>
        /// <param name="buttonType">Button Type</param>
        /// <returns>Result</returns>
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
    }
}
