using System.Collections.Generic;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.TextTable
{
    public class LocalizedTextTable : LocalizedTable
    {
        [SerializeField] public List<LocalizedTextTableEntry> entries;

        public LocalizedTextTable()
        {
            entries = new List<LocalizedTextTableEntry>();
        }

        public override void AddNewEntry() => entries.Add(new LocalizedTextTableEntry());
        public void AddNewEntry(LocalizedTextTableEntry entry) => entries.Add(entry);
        

        public override void RemoveEntryAt(int index) => entries.RemoveAt(index);

        public override int EntryCount => entries.Count;
    }
}