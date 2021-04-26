using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime
{
    public static class LocalizeLoader
    {
        public static string GetText(LocalizedTextTable table, string key)
        {
            if (table == null)
                return "";
            
            var entry = table.entries.Find(e => e.key == key);
            return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
        }

        public static Sprite GetSprite(LocalizedImageTable table, string key)
        {
            if (table == null)
                return null;
            
            var entry = table.entries.Find(e => e.key == key);
            return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
        }
        
    }
}
