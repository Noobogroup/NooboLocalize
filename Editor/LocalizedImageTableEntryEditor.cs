using NooboPackage.NooboLocalize.Runtime.ImageTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedImageTableEntry))]
    public class LocalizedImageTableEntryEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var locales = Resources.LoadAll<Locale>("NooboLocalize/Locales");
            
            var keys = property.FindPropertyRelative("translation.keys");
            keys.arraySize = locales.Length;
            var values = property.FindPropertyRelative("translation.values");
            values.arraySize = locales.Length;
            property.serializedObject.ApplyModifiedProperties();
            
            var root = new VisualElement();
            root.AddToClassList("table-row-content-parent");

            var id = new TextField {bindingPath = "key"};
            id.multiline = true;
            id.AddToClassList("table-row-content");
            root.Add(id);
            
            for(var i = 0; i < locales.Length; i++)
            {
                keys.GetArrayElementAtIndex(i).stringValue = locales[i].name;
                
                var p = new VisualElement {style = { flexBasis = 0, flexGrow = 1, flexDirection = FlexDirection.Column, alignItems= Align.Center}};
                var value = new ObjectField();
                value.objectType = typeof(Sprite);
                value.BindProperty(values.GetArrayElementAtIndex(i));

                var preview = new Image();
                preview.image = ((Sprite) values.GetArrayElementAtIndex(i).objectReferenceValue)?.texture;
                preview.AddToClassList("image-table-row-content");
                
                p.Add(value);
                p.Add(preview);
                root.Add(p);

                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }

            
            
            return root;
        }
    }
}

