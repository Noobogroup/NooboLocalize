using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.TextTable
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMPText : MonoBehaviour
    {
        [SerializeField] public LocalizedTextReference id;
        [SerializeField] private bool inheritAlignment;
        [SerializeField] private bool inheritFont = true;

        [SerializeField] private TMP_Text targetComponent;

        private void Awake()
        {
            LocalizeUpdate();
        }

        public void LocalizeUpdate()
        {
            if (inheritAlignment)
            {
                targetComponent.alignment = NooboLocalizeSettings.CurrentLocale().align;
                if (NooboLocalizeSettings.CurrentLocale().isRtl)
                    targetComponent.isRightToLeftText = true;
            }

            if (inheritFont)
            {
                var m = new Material(NooboLocalizeSettings.CurrentLocale().customTMPFont.material);
                TMP_MaterialManager.CopyMaterialPresetProperties(targetComponent.fontMaterial, m);
                targetComponent.font = NooboLocalizeSettings.CurrentLocale().customTMPFont;
                targetComponent.fontMaterial = m;
            }

            if (id != null && id.table != null)
                SetText(LocalizeLoader.GetText(id.table, id.key));
        }

        public void SetText(string text)
        {
            if (NooboLocalizeSettings.CurrentLocale().isRtl && inheritAlignment)
                targetComponent.SetText(new string(text.Reverse().ToArray()));
            else
                targetComponent.SetText(text);
        }

        private void OnValidate()
        {
            targetComponent = GetComponent<TMP_Text>();
        }
    }
}