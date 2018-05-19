using RealmListManager.UI.Core.Models;

namespace RealmListManager.UI.Core.Events
{
    public class LocationChanged
    {
        public bool IsDeleted { get; set; }
        public LocationModel Location { get; set; }
    }
}
