using System;
using System.IO;
using System.Windows.Media;
using Caliburn.Micro;
using RealmListManager.UI.Core.Entities;
using RealmListManager.UI.Core.Utilities;

namespace RealmListManager.UI.Core.Models
{
    public class RealmlistModel : PropertyChangedBase
    {
        #region Fields

        private string _imagePath;
        private ImageSource _image;

        #endregion

        #region Constructor

        public RealmlistModel(Realmlist dataModel = null)
        {
            DataModel = dataModel ?? new Realmlist {Id = Guid.NewGuid()};

            if (DataModel.Image != null)
                _image = ImageUtilities.Deserialize(DataModel.Image);
        }

        #endregion

        #region Properties

        public Realmlist DataModel { get; }

        public bool UrlValid => Url == null || (Uri.IsWellFormedUriString(Url, UriKind.RelativeOrAbsolute) && !string.IsNullOrWhiteSpace(Url));

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

        public string Url
        {
            get => DataModel.Url;
            set
            {
                if (DataModel.Url == value) return;
                DataModel.Url = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => UrlValid);
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
            set
            {
                if (Equals(_image, value)) return;
                _image = value;
                DataModel.Image = ImageUtilities.Serialize(_image);
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public RealmlistModel Clone()
        {
            var clone = new Realmlist
            {
                Id = DataModel.Id,
                Name = DataModel.Name,
                Image = DataModel.Image,
                Url = DataModel.Url
            };

            return new RealmlistModel(clone);
        }
    }
}
