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
            Realmlist = new RealmlistModel();
            Realmlist.PropertyChanged += Realmlist_PropertyChanged;
        }

        #endregion

        #region Properties

        public RealmlistModel Realmlist { get; set; }

        public bool Result { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(Realmlist.Name) &&
                               !string.IsNullOrWhiteSpace(Realmlist.Url);

        #endregion

        #region Actions

        public void BrowseImagePath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Image files", "*.png;*.jpg"));
            dialog.Filters.Add(new CommonFileDialogFilter("Executable files", "*.exe"));

            var result = dialog.ShowDialog(GetView() as Window);

            if (result != CommonFileDialogResult.Ok) return;

            Realmlist.ImagePath = dialog.FileName;
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

        private void Realmlist_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanSave);
        }

        #endregion
    }
}
