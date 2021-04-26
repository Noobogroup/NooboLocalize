using System.Collections.Generic;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.ImageTable
{
    public class LocalizedImageTable : LocalizedTable
    {
        [SerializeField] public List<LocalizedImageTableEntry> entries;
        
        public LocalizedImageTable()
        {
            entries = new List<LocalizedImageTableEntry>();
        }

        public override void AddNewEntry() => entries.Add(new LocalizedImageTableEntry());
        public override void RemoveEntryAt(int index) => entries.RemoveAt(index);

        public override int EntryCount => entries.Count;
    }
}
