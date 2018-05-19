using System.Windows;
using Caliburn.Micro;
using Microsoft.WindowsAPICodePack.Dialogs;
using RealmListManager.UI.Core.Models;

namespace RealmListManager.UI.Dialogs
{
    public class NewLocationViewModel : Screen
    {
        #region Constructor

        public NewLocationViewModel()
        {
            Location = new LocationModel();
            Location.PropertyChanged += Location_PropertyChanged;
        }

        #endregion

        #region Properties

        public LocationModel Location { get; set; }

        public bool Result { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(Location.Name) &&
                               !string.IsNullOrWhiteSpace(Location.Path) &&
                               Location.PathValid && Location.ImagePathValid;

        #endregion

        #region Actions

        public void BrowsePath()
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            Location.Path = dialog.FileName;
        }

        public void BrowseImagePath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.png;*.jpg"));
            dialog.Filters.Add(new CommonFileDialogFilter("Executable files", "*.exe"));

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            Location.ImagePath = dialog.FileName;
        }

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

        private void Location_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanSave);
        }

        #endregion
    }
}
