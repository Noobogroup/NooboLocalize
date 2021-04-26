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
        
        private TMP_Text _targetComponent;
        private bool _initialized = false;
        private void Awake()
        {
            Initialize();
            LocalizeUpdate();
        }

        private void Initialize()
        {
            if(_initialized)
                return;

            _initialized = true;
            _targetComponent = GetComponent<TMP_Text>();
        }
        

        internal void LocalizeUpdate()
        {
            Initialize();
            
            _targetComponent.text = LocalizeLoader.GetText(id.table, id.key);
            if (inheritAlignment)
                _targetComponent.alignment = NooboLocalizeSettings.CurrentLocale().align;
            if(inheritFont)
                _targetComponent.font = NooboLocalizeSettings.CurrentLocale().customTMPFont;
        }
    }
}
