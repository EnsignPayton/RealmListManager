using System;
using System.IO;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace RealmListManager.UI.Core
{
    public class ConfigurationManager : PropertyChangedBase, IConfigurationManager
    {
        private const string FileName = "RealmListManager.xml";

        private AppSettings _appSettings;

        public bool RestoreRealmlist
        {
            get => _appSettings.RestoreRealmlist;
            set
            {
                _appSettings.RestoreRealmlist = value;
                NotifyOfPropertyChange();
            }
        }

        public bool MinimizeToTray
        {
            get => _appSettings.MinimizeToTray;
            set
            {
                _appSettings.MinimizeToTray = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CloseToTray
        {
            get => _appSettings.CloseToTray;
            set
            {
                _appSettings.CloseToTray = value;
                NotifyOfPropertyChange();
            }
        }

        public void Save()
        {
            var file = Path.Combine(Environment.CurrentDirectory, FileName);
            using (var streamWriter = new StreamWriter(file))
            {
                var serializer = new XmlSerializer(typeof(AppSettings));
                serializer.Serialize(streamWriter, _appSettings);
            }
        }

        public void Load()
        {
            var file = Path.Combine(Environment.CurrentDirectory, FileName);
            try
            {
                using (var streamReader = new StreamReader(file))
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    _appSettings = serializer.Deserialize(streamReader) as AppSettings;
                }
            }
            catch (FileNotFoundException ex)
            {
                _appSettings = new AppSettings();
            }
        }
    }
}
