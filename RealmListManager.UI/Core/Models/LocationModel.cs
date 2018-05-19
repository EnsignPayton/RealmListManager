using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using RealmListManager.UI.Core.Entities;
using RealmListManager.UI.Core.Utilities;

namespace RealmListManager.UI.Core.Models
{
    public class LocationModel : PropertyChangedBase
    {
        #region Fields

        private string _imagePath;
        private ImageSource _image;
        private ObservableCollection<RealmlistModel> _realmlists;

        #endregion

        #region Constructor

        public LocationModel(Location dataModel = null)
        {
            DataModel = dataModel ?? new Location {Id = Guid.NewGuid()};

            if (DataModel.Image != null)
                _image = ImageUtilities.Deserialize(DataModel.Image);

            if (DataModel.Realmlists != null)
            {
                var models = DataModel.Realmlists.Select(x => new RealmlistModel(x));
                _realmlists = new ObservableCollection<RealmlistModel>(models);
            }
            else
            {
                _realmlists = new ObservableCollection<RealmlistModel>();
            }

            _realmlists.CollectionChanged += Realmlists_CollectionChanged;
        }

        #endregion

        #region Properties

        public Location DataModel { get; }

        public bool PathValid => Path == null || FileUtilities.IsWowFolder(Path);

        public bool ImagePathValid => ImagePath == null || File.Exists(ImagePath);

        public string Name
        {
            get => DataModel.Name;
            set
            {
                if (DataModel.Name == value) return;
                DataModel.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Path
        {
            get => DataModel.Path;
            set
            {
                if (DataModel.Path == value) return;
                DataModel.Path = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => PathValid);
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath == value) return;
                _imagePath = value;

                Image = ImagePathValid && _imagePath != null ? ImageUtilities.ParseImageFile(_imagePath) : null;

                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => ImagePathValid);
            }
        }

        public ImageSource Image
        {
            get => _image;
            private set
            {
                if (Equals(_image, value)) return;
                _image = value;
                DataModel.Image = ImageUtilities.Serialize(_image);
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<RealmlistModel> Realmlists
        {
            get => _realmlists;
            set
            {
                if (_realmlists == value) return;
                _realmlists = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Methods

        public LocationModel Clone()
        {
            // TODO: Use serialization to make this general for all models
            var clone = new Location
            {
                Id = DataModel.Id,
                Name = DataModel.Name,
                Path = DataModel.Path,
                Image = DataModel.Image,
                Realmlists = new List<Realmlist>()
            };

            foreach (var realmlist in Realmlists)
            {
                clone.Realmlists.Add(realmlist.DataModel);
            }

            return new LocationModel(clone);
        }

        private void Realmlists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var realmlist in Realmlists)
            {
                if (DataModel.Realmlists == null)
                    DataModel.Realmlists = new List<Realmlist>();

                if (DataModel.Realmlists.Contains(realmlist.DataModel))
                    continue;

                DataModel.Realmlists.Add(realmlist.DataModel);
            }
        }

        #endregion
    }
}
