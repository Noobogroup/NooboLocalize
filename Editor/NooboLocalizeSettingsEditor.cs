using System.Collections.Generic;
using System.IO;
using System.Linq;
using NooboPackage.NooboLocalize.Runtime;
using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomEditor(typeof(NooboLocalizeSettings))]
    public class NooboLocalizeSettingsEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var tree = Resources.Load<VisualTreeAsset>("EditorResources/Settings/settings");
            var root = tree.CloneTree();

            var locales = new List<string>();
            locales.AddRange(Resources.LoadAll<Locale>("NooboLocalize/Locales").Select(locale => locale.name).ToList());


            var field = new PopupField<string>(
                "Current Locale",
                locales,
                locales[0]);
            try
            {
                field.BindProperty(serializedObject.FindProperty("selectedLocale"));
            }
            catch
            {
                serializedObject.FindProperty("selectedLocale").stringValue = locales[0];
                serializedObject.ApplyModifiedProperties();
                field.BindProperty(serializedObject.FindProperty("selectedLocale"));
            }

            root.Q<VisualElement>("locale-selector").Add(field);

            CreateTable(root);

            var b = root.Q<Button>("AddTextTable");
            b.clicked += AddNewTable<LocalizedTextTable>;
            b.clicked += () => CreateTable(root);

            b = root.Q<Button>("AddImageTable");
            b.clicked += AddNewTable<LocalizedImageTable>;
            b.clicked += () => CreateTable(root);

            return root;
        }

        private void AddNewTable<T>() where T : LocalizedTable
        {
            var s = CreateInstance<T>();
            Directory.CreateDirectory("Assets/Resources/NooboLocalize/Tables/");

            var i = 0;
            while (File.Exists($"Assets/Resources/NooboLocalize/Tables/New{typeof(T).Name}_{i}.asset"))
                i++;

            AssetDatabase.CreateAsset(s, $"Assets/Resources/NooboLocalize/Tables/New{typeof(T).Name}_{i}.asset");
            AssetDatabase.SaveAssets();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void CreateTable(VisualElement root)
        {
            var container = root.Q<VisualElement>("tables-container");
            while(container.childCount > 0)
                container.RemoveAt(0);
            
            var tables = serializedObject.FindProperty("tables");
            if (tables.arraySize < 1)
            {
                var b = new Box();
                b.style.paddingBottom = b.style.paddingLeft = b.style.paddingRight = b.style.paddingTop = 9;
                b.style.marginTop = b.style.marginBottom = 5;
                b.Add(new Label("Theres no Table here."));
                container.Add(b);
            }
            else
            {
                var listItems = new List<SerializedProperty>();
                for (var i = 0; i < tables.arraySize; i++)
                    listItems.Add(tables.GetArrayElementAtIndex(i));
                var list = new ListView(listItems,
                    35,
                    () =>
                    {
                        var v = new VisualElement();
                        v.style.flexDirection = FlexDirection.Row;
                        v.Add(new Label
                        {
                            name = "id",
                            style =
                            {
                                unityTextAlign = TextAnchor.MiddleCenter, 
                                paddingRight = 10, 
                                paddingLeft = 10,
                                backgroundColor = new StyleColor(new Color(0.31f, 0.33f, 0.35f)),
                                marginRight = 2,
                                marginTop = 1,
                                marginBottom = 1,
                                flexBasis = 0
                            }
                        });
                        v.Add(new PropertyField {name = "content", style = { flexGrow = 1}});
                        
                        return v;
                    },
                    (element, i) =>
                    {
                        element.Q<Label>("id").text = i + "";
                        element.Q<PropertyField>("content").bindingPath = $"tables.Array.data[{i}]";
                        element.Q<PropertyField>("content").Bind(serializedObject);
                    });
                
                list.style.height = tables.arraySize * 35 < 370 ? tables.arraySize * 35 : 370;
                list.selectionType = SelectionType.None;
                container.Add(list);
            }
        }


        [MenuItem("Window/Noobo Localize", priority = -100)]
        private static void SelectSettings()
        {
            var settings = Resources.Load<NooboLocalizeSettings>("NooboLocalize/Settings");
            if (settings == null)
            {
                Initialize();
                settings = Resources.Load<NooboLocalizeSettings>("NooboLocalize/Settings");
                Selection.SetActiveObjectWithContext(settings, null);
                return;
            }

            Selection.SetActiveObjectWithContext(settings, null);
        }

        private static void Initialize()
        {
            var s = CreateInstance<NooboLocalizeSettings>();
            s.selectedLocale = "(None)";
            Directory.CreateDirectory("Assets/Resources/NooboLocalize/");
            Directory.CreateDirectory("Assets/Resources/NooboLocalize/Tables");
            Directory.CreateDirectory("Assets/Resources/NooboLocalize/Locales");

            AssetDatabase.CreateAsset(s,
                "Assets/Resources/NooboLocalize/Settings.asset");
            AssetDatabase.CreateAsset(CreateInstance<Locale>(),
                "Assets/Resources/NooboLocalize/Locales/fa-IR.asset");
            AssetDatabase.CreateAsset(CreateInstance<Locale>(),
                "Assets/Resources/NooboLocalize/Locales/en-US.asset");
            AssetDatabase.SaveAssets();
        }
    }
}