using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using System.Text.RegularExpressions;
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

        public static string GetText(LocalizedTextTable table, string key,
            params LocalizedTextTableEntry[] formatOptions)
        {
            if (table == null)
                return "";

            var entry = table.entries.Find(e => e.key == key);
            if (entry == null)
            {
                Debug.LogError($"No entry with key '{key}' found. [Table: {table.name}]");
                return "";
            }

            var currentLocale = NooboLocalizeSettings.CurrentLocale();
            var result = entry.translation[currentLocale.name];
            
            if(formatOptions == null)
                return result;

            foreach (var format in formatOptions)
            {
                result = Regex.Replace(result, @"{\s*" + format.key + @"\s*}", format.translation[currentLocale.name]);
            }

            return result;
        }

        public static string GetText(string tableName, string key, params LocalizedTextTableEntry[] formatOptions)
        {
            if (_loadedTextTables.ContainsKey(tableName))
                return GetText(_loadedTextTables[tableName], key, formatOptions);

            var table = Resources.Load<LocalizedTextTable>($"NooboLocalize/Tables/{tableName}");
            if (table == null)
            {
                Debug.LogError($"No text table named '{tableName}' found.");
                return "";
            }

            _loadedTextTables.Add(tableName, table);
            return GetText(table, key, formatOptions);
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