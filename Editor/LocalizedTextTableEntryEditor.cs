using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedTextTableEntry))]
    public class LocalizedTextTableEntryEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            
            
            var root = new VisualElement();
            root.AddToClassList("table-row-content-parent");

            var id = new TextField {bindingPath = "key"};
            id.multiline = true;
            id.AddToClassList("table-row-content");
            root.Add(id);
            
            
            var locales = Resources.LoadAll<Locale>("NooboLocalize/Locales");
            
            var keys = property.FindPropertyRelative("translation.keys");
            keys.arraySize = locales.Length;
            var values = property.FindPropertyRelative("translation.values");
            values.arraySize = locales.Length;
            
            property.serializedObject.ApplyModifiedProperties();
            
            for(var i = 0; i < locales.Length; i++)
            {
                keys.GetArrayElementAtIndex(i).stringValue = locales[i].name;
               
                var value = new TextField();
                value.multiline = true;
                value.AddToClassList("table-row-content");
                value.BindProperty(values.GetArrayElementAtIndex(i));
                root.Add(value);
            }
            
            property.serializedObject.ApplyModifiedProperties();
            
            return root;
        }
    }
}
