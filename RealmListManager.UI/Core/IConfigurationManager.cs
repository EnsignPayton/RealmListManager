using System.ComponentModel;

namespace RealmListManager.UI.Core
{
    public interface IConfigurationManager : INotifyPropertyChanged
    {
        bool RestoreRealmlist { get; set; }
        bool MinimizeToTray { get; set; }
        bool CloseToTray { get; set; }

        void Save();
        void Load();
    }
}
