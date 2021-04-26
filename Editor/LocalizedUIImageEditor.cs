using System.Linq;
using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomEditor(typeof(LocalizedUIImage))]
    public class LocalizedUIImageEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var table = new ObjectField {label = "Table", bindingPath = "id.table"};
            table.Bind(serializedObject);
            table.objectType = typeof(LocalizedImageTable);
            table.RegisterCallback<ChangeEvent<Object>>((e) =>
            {
                if (e.newValue == null)
                {
                    NoTable(root);
                    return;
                }

                ((LocalizedUIImage) target).id.table = (LocalizedImageTable) e.newValue;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                CreateTableKeySelector(root);
            });
            root.Add(table);

            var v = new VisualElement();
            var b = new Box {name = "container"};
            b.style.paddingBottom = b.style.paddingLeft = b.style.paddingRight = b.style.paddingTop = 10;
            b.style.marginTop = 10;

            v.Add(b);
            root.Add(v);

            if (table.value == null)
            {
                NoTable(root);
                return root;
            }

            CreateTableKeySelector(root);
            return root;
        }

        private void NoTable(VisualElement root)
        {
            var b = root.Q<Box>("container");

            while (b.childCount > 0)
                b.RemoveAt(0);

            b.Add(new Label("no image table selected."));
        }

        private void CreateTableKeySelector(VisualElement root)
        {
            var t = target as LocalizedUIImage;
            var b = root.Q<Box>("container");
            if (t.id.table == null)
            {
                NoTable(root);
                return;
            }

            var keys = t.id.table.entries.Select(entry => entry.key).ToList();
            if (keys.Count < 1)
            {
                while (b.childCount > 0)
                    b.RemoveAt(0);

                b.Add(new Label("There's no key in selected image table."));
                return;
            }

            var keyOptions = new PopupField<string>(keys, 0);
            keyOptions.label = "Key";
            try
            {
                keyOptions.BindProperty(serializedObject.FindProperty("id.key"));
            }
            catch
            {
                serializedObject.FindProperty("id.key").stringValue = keys[0];
                serializedObject.ApplyModifiedProperties();
                keyOptions.BindProperty(serializedObject.FindProperty("id.key"));
            }

            while (b.childCount > 0)
                b.RemoveAt(0);

            b.Add(keyOptions);
        }
    }
}
