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
            NewLocation = new LocationModel();
            NewLocation.PropertyChanged += NewLocation_PropertyChanged;
        }

        #endregion

        #region Properties

        public LocationModel NewLocation { get; }

        public bool Result { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(NewLocation.Name) &&
                               !string.IsNullOrWhiteSpace(NewLocation.Path) &&
                               NewLocation.PathValid && NewLocation.ImagePathValid;

        #endregion

        #region Actions

        public void BrowsePath()
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            NewLocation.Path = dialog.FileName;
        }

        public void BrowseImagePath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.png;*.jpg"));
            dialog.Filters.Add(new CommonFileDialogFilter("Executable files", "*.exe"));

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            NewLocation.ImagePath = dialog.FileName;
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

        private void NewLocation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanSave);
        }

        #endregion
    }
}
