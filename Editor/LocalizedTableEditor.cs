using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FileHelpers.Options;
using NooboPackage.NooboLocalize.Runtime;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomEditor(typeof(LocalizedTable), true)]
    public class LocalizedTableEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var tree = Resources.Load<VisualTreeAsset>("EditorResources/TextTable/root");
            var style = Resources.Load<StyleSheet>("EditorResources/TextTable/style");
            var root = tree.CloneTree();
            root.styleSheets.Add(style);

            root.Q<Label>("text-table-header").text = Selection.activeObject.name;
            
            CreateHeaders(root);
            UpdateEntries(root);
            CreateButtons(root);
            
            return root;
        }

        private void CreateHeaders(VisualElement root)
        {
            var locales = Resources.LoadAll<Locale>("NooboLocalize/Locales");
            var tableHeaders = root.Q<VisualElement>("header");
            foreach (var locale in locales)
            {
                tableHeaders.Add(NewHeader(locale));
            }
        }

        private void UpdateEntries(VisualElement root)
        {
            var entries = serializedObject.FindProperty("entries");
            var listItems = new List<SerializedProperty>();
            for (var i = 0; i < entries.arraySize; i++)
            {
                listItems.Add(entries.GetArrayElementAtIndex(i));
            }

            var entriesParent = root.Q<VisualElement>("rows");
            
            for(var i = 0; i < entriesParent.childCount; i++)
                entriesParent.RemoveAt(i);

            var list = new ListView(
                listItems,
                80,
                () => new PropertyField(),
                (element, i) =>
                {
                    ((PropertyField) element).bindingPath = $"entries.Array.data[{i}]";
                    ((PropertyField) element).BindProperty(serializedObject);
                });
            list.name = "TableList";
            list.style.height = entries.arraySize * 80 < 370 ? entries.arraySize * 80 : 370;
            entriesParent.Add(list);
        }

        private void CreateButtons(VisualElement root)
        {
            root.Q<Button>("remove-entry").clicked += () =>
            {
                var table = target as LocalizedTable;
                if (table == null)
                    return;

                var list = root.Q<ListView>("TableList");
                if (list.selectedIndex < 0)
                    return;

                table.RemoveEntryAt(list.selectedIndex);

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                UpdateEntries(root);
            };

            root.Q<Button>("add-entry").clicked += () =>
            {
                var table = target as LocalizedTable;
                if (table == null)
                    return;

                table.AddNewEntry();

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                UpdateEntries(root);
            };

            root.Q<Button>("back").clicked += () =>
            {
                Selection.SetActiveObjectWithContext(
                    Resources.Load<NooboLocalizeSettings>("NooboLocalize/Settings"), null);
            };

            root.Q<Button>("btn-rename").clicked += () =>
            {
                AssetDatabase.RenameAsset($"Assets/Resources/NooboLocalize/Tables/{target.name}.asset",
                    root.Q<TextField>("rename").text + ".asset");
                root.Q<Label>("text-table-header").text = Selection.activeObject.name;
            };

            root.Q<Button>("btn-remove").clicked += () =>
            {
                var result = EditorUtility.DisplayDialog("Remove Table",
                    "Are you sure you want to remove this table? all data on table and links on objects will be gone.",
                    "yes, i'm sure", "no i think . . .");

                if (!result)
                    return;

                AssetDatabase.DeleteAsset($"Assets/Resources/NooboLocalize/Tables/{target.name}.asset");
                AssetDatabase.SaveAssets();
                Selection.SetActiveObjectWithContext(
                    Resources.Load<NooboLocalizeSettings>("NooboLocalize/Settings"),
                    null);
            };

            if (target is LocalizedTextTable)
                root.Q<Button>("btn-import-csv").clicked += () => ImportFromCsv(root);
            else
                root.Q<Button>("btn-import-csv").style.visibility = Visibility.Hidden;
        }


        private void ImportFromCsv(VisualElement root)
        {
            if (target is LocalizedTextTable table)
            {
                while(table.EntryCount > 0)
                    table.RemoveEntryAt(0);
            }
            else
                return;

            var addr = EditorUtility.OpenFilePanel("Import from csv file", Application.dataPath, "csv");
            
            if(string.IsNullOrEmpty(addr))
                return;

            // var data = File.ReadAllLines(addr);
            
            var data = FileHelpers.CsvEngine.CsvToDataTable(addr, ',');
            
            CsvCheckHeaders(data.Columns);

            for(var j = 0; j < data.Rows.Count; j++)
            {
                var entry = new LocalizedTextTableEntry();
                var lineData = data.Rows[j].ItemArray.Cast<string>().ToList();

                entry.key = lineData[0];

                for (var i = 1; i < lineData.Count; i++)
                {
                    entry.translation[data.Columns[i].Caption.Replace('_','-')] = lineData[i];
                }
                
                (target as LocalizedTextTable)?.AddNewEntry(entry);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
            EditorUtility.SetDirty(serializedObject.targetObject);
            
            UpdateEntries(root);
        }

        private void CsvCheckHeaders(DataColumnCollection headers)
        {
            if (headers[0].Caption != "key")
                throw new Exception("No 'key' in headers.");
            
            var locales = Resources.LoadAll<Locale>("NooboLocalize/Locales");
            
            for(var i = 1; i < headers.Count; i++)
                if (!locales.Select(l => l.name).Contains(headers[i].Caption.Replace('_', '-')))
                    throw new Exception($"Bad locale in csv file. {headers[i].Caption.Replace('_', '-')}");
        }

        private VisualElement NewHeader(Locale locale)
        {
            var header = new TextElement {text = locale.name};
            header.AddToClassList("table-content-header");
            return header;
        }
    }
}
