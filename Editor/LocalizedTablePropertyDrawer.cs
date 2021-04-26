using NooboPackage.NooboLocalize.Runtime;
using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedTable), true)]
    public class LocalizedTablePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = Resources.Load<VisualTreeAsset>("EditorResources/Settings/table_property").CloneTree();

            var img = root.Q<Image>("icon");
            switch (property.objectReferenceValue)
            {
                case LocalizedTextTable _:
                    img.image = Resources.Load<Texture>("EditorResources/Images/TextTable");
                    break;
                case LocalizedImageTable _:
                    img.image = Resources.Load<Texture>("EditorResources/Images/ImageTable");
                    break;
            }
            
            var l = root.Q<Label>("title");
            l.text = property.objectReferenceValue.name;

            var b = root.Q<Button>("edit");
            b.clicked += () =>
            {
                Selection.SetActiveObjectWithContext(property.objectReferenceValue, null);
            };

            return root;
        }
    }
}
