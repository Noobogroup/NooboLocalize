using TMPro;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.TextTable
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMPText : MonoBehaviour
    {
        [SerializeField] public LocalizedTextReference id; 
        [SerializeField] private bool inheritAlignment = true;
        [SerializeField] private bool inheritFont = true;
        
        [SerializeField] private TMP_Text targetComponent;
        private void Awake()
        {
            LocalizeUpdate();
        }
        
        public void LocalizeUpdate()
        {
            if (inheritAlignment)
                targetComponent.alignment = NooboLocalizeSettings.CurrentLocale().align;
            if(inheritFont)
                targetComponent.font = NooboLocalizeSettings.CurrentLocale().customTMPFont;
            
            if(id != null && id.table != null)
                targetComponent.text = LocalizeLoader.GetText(id.table, id.key);
        }

        private void OnValidate()
        {
            targetComponent = GetComponent<TMP_Text>();
        }
    }
}
