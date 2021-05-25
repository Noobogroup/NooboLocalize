using System.Linq;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NooboPackage.NooboLocalize.Editor
{
    [CustomEditor(typeof(LocalizedTMPText))]
    public class LocalizedTMPTextEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var table = new ObjectField {label = "Table", bindingPath = "id.table"};
            table.Bind(serializedObject);
            table.objectType = typeof(LocalizedTextTable);
            table.RegisterCallback<ChangeEvent<Object>>((e) =>
            {
                if (e.newValue == null)
                {
                    NoTable(root);
                    return;
                }

                ((LocalizedTMPText) target).id.table = (LocalizedTextTable) e.newValue;
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

            b.Add(new Label("no text table selected.")
                {style = {unityTextAlign = TextAnchor.MiddleCenter, marginBottom = 10}});

            AddAlignAndFont(b);
        }

        private void NoKeyInTable(VisualElement root)
        {
            var b = root.Q<Box>("container");

            while (b.childCount > 0)
                b.RemoveAt(0);

            b.Add(new Label("There's no key in selected text table.")
                {style = {unityTextAlign = TextAnchor.MiddleCenter, marginBottom = 10}});

            AddAlignAndFont(b);
        }


        private void AddAlignAndFont(VisualElement b)
        {
            var align = new Toggle("Inherit Alignment") {bindingPath = "inheritAlignment"};
            b.Add(align);
            align.Bind(serializedObject);
            align.binding.Update();
            
            var direction = new Toggle("Inherit Direction") {bindingPath = "inheritDirection"};
            b.Add(direction);
            direction.Bind(serializedObject);
            direction.binding.Update();

            var font = new Toggle("Inherit Font") {bindingPath = "inheritFont"};
            b.Add(font);
            font.Bind(serializedObject);
            font.binding.Update();
        }

        private void CreateTableKeySelector(VisualElement root)
        {
            var t = target as LocalizedTMPText;
            var b = root.Q<Box>("container");
            if (t.id.table == null)
            {
                NoTable(root);
                return;
            }

            var keys = t.id.table.entries.Select(entry => entry.key).ToList();
            if (keys.Count < 1)
            {
                NoKeyInTable(root);
                return;
            }

            var keyOptions = new PopupField<string>(keys, 0) {style = {marginBottom = 10}};
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

            AddAlignAndFont(b);
        }
    }
}