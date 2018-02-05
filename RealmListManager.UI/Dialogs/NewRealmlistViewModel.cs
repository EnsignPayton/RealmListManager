using System.Windows;
using Caliburn.Micro;
using Microsoft.WindowsAPICodePack.Dialogs;
using RealmListManager.UI.Core.Models;

namespace RealmListManager.UI.Dialogs
{
    public class NewRealmlistViewModel : Screen
    {
        #region Constructor

        public NewRealmlistViewModel()
        {
            NewRealmlist = new RealmlistModel();
            NewRealmlist.PropertyChanged += NewRealmlist_PropertyChanged;
        }

        #endregion

        #region Properties

        public RealmlistModel NewRealmlist { get; }

        public bool Result { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(NewRealmlist.Name) &&
                               !string.IsNullOrWhiteSpace(NewRealmlist.Url);

        #endregion

        #region Actions

        public void BrowseImagePath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.png;*.jpg"));
            dialog.Filters.Add(new CommonFileDialogFilter("Executable files", "*.exe"));

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            NewRealmlist.ImagePath = dialog.FileName;
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

        private void NewRealmlist_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanSave);
        }

        #endregion
    }
}
