using System.Collections.ObjectModel;
using Caliburn.Micro;
using RealmListManager.UI.Models;

namespace RealmListManager.UI.Shell
{
    public class ShellViewModel : Conductor<IScreen>
    {
        #region Constructor

        public ShellViewModel()
        {
            Locations = new ObservableCollection<LocationModel>();
        }

        #endregion

        #region Properties

        public ObservableCollection<LocationModel> Locations { get; set; }

        #endregion

        #region Actions

        public void AddLocation()
        {
            var rl1 = new RealmlistModel
            {
                Name = "Foo",
                Url = "Bar",
                ImagePath = "Baz"
            };

            var loc1 = new LocationModel()
            {
                Name = "Foo",
                Path = "Bar",
                ImagePath = "Baz",
                Realmlists = new ObservableCollection<RealmlistModel>()
            };

            loc1.Realmlists.Add(rl1);

            Locations.Add(loc1);
        }

        #endregion
    }
}
