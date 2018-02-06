﻿using System;
using System.IO;
using System.Windows.Media;
using Caliburn.Micro;
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

        public RealmlistModel(Entities.Realmlist dataModel = null)
        {
            DataModel = dataModel ?? new Entities.Realmlist {Id = Guid.NewGuid()};

            if (DataModel.Image != null)
                _image = ImageUtilities.Deserialize(DataModel.Image);
        }

        #endregion

        #region Properties

        public Entities.Realmlist DataModel { get; }

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
    }
}