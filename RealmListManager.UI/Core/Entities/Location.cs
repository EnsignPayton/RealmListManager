using System;
using System.Collections.Generic;

namespace RealmListManager.UI.Core.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Image { get; set; }
        public IList<Realmlist> Realmlists { get; set; }
    }
}
