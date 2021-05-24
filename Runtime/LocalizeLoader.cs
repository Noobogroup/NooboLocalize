using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime
{
    public static class LocalizeLoader
    {
        private static Dictionary<string, LocalizedTextTable> _loadedTextTables;
        private static Dictionary<string, LocalizedImageTable> _loadedImageTables;

        static LocalizeLoader()
        {
            _loadedTextTables = new Dictionary<string, LocalizedTextTable>();
            _loadedImageTables = new Dictionary<string, LocalizedImageTable>();
        }
        
        public static string GetText(LocalizedTextTable table, string key)
        {
            if (table == null)
                return "";
            
            var entry = table.entries.Find(e => e.key == key);
            return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
        }
        
        public static string GetText(string tableName, string key)
        {
            if (_loadedTextTables.ContainsKey(tableName))
            {
                var entry = _loadedTextTables[tableName].entries.Find(e => e.key == key);
                return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
            }
            else
            {
                var table = Resources.Load<LocalizedTextTable>($"NooboLocalize/Tables/{tableName}");
                if (table == null)
                {
                    Debug.LogError($"No text table named '{tableName}' found.");
                    return "";
                }
                
                _loadedTextTables.Add(tableName, table);
                var entry = table.entries.Find(e => e.key == key);
                return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
            }
        }
        
        
        public static Sprite GetSprite(LocalizedImageTable table, string key)
        {
            if (table == null)
                return null;
            
            var entry = table.entries.Find(e => e.key == key);
            return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
        }

        public static Sprite GetSprite(string tableName, string key)
        {
            if (_loadedImageTables.ContainsKey(tableName))
            {
                var entry = _loadedImageTables[tableName].entries.Find(e => e.key == key);
                return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
            }
            else
            {
                var table = Resources.Load<LocalizedImageTable>($"NooboLocalize/Tables/{tableName}");
                if (table == null)
                {
                    Debug.LogError($"No image table named '{tableName}' found.");
                    return null;
                }

                _loadedImageTables.Add(tableName, table);
                var entry = table.entries.Find(e => e.key == key);
                return entry.translation[NooboLocalizeSettings.CurrentLocale().name];
            }
        }
    }
}


