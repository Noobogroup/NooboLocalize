using System;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime
{
    [Serializable]
    public class LocalizedTable : ScriptableObject
    {
        public virtual void AddNewEntry() { }

        public virtual void RemoveEntryAt(int index) { }

        public virtual int EntryCount => 0;
    }
}
