using System;

namespace RealmListManager.UI.Core.Entities
{
    public class Realmlist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public int Index { get; set; }
    }
}
