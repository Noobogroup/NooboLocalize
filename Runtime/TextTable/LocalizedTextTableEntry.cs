using System;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.TextTable
{
    [Serializable]
    public class LocalizedTextTableEntry
    {
        [SerializeField] public string key;
        [SerializeField] public Translation translation;

        public LocalizedTextTableEntry()
        {
            key = "NewEntry";
            translation = new Translation();
        }

        public LocalizedTextTableEntry(string key, Translation translation)
        {
            this.key = key;
            this.translation = translation;
        }
    }
}
