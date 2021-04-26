using System;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.ImageTable
{
    [Serializable]
    public class LocalizedImageTableEntry
    {
        [SerializeField] public string key;
        [SerializeField] public ImageTranslation translation;

        public LocalizedImageTableEntry()
        {
            key = "New Entry";
            translation = new ImageTranslation();
        }
    }
}
