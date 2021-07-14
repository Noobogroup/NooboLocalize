using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "Settings", menuName = "NooboPackage/Localization/Settings")]
    public class NooboLocalizeSettings : ScriptableObject, ISerializationCallbackReceiver
    {
        private static NooboLocalizeSettings _;
        
        [SerializeField] public string selectedLocale;
        [SerializeField] public List<LocalizedTable> tables;

        private List<Locale> _locales;
        
        public static Locale[] Locales => _._locales.ToArray();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var settings = Resources.Load<NooboLocalizeSettings>("NooboLocalize/settings");
            if (settings == null)
            {
                throw new Exception(
                    "No NooboLocalize Settings exist! try to create one by clicking on Window/NooboLocalize in toolbar.");
            }
            
            settings.Set();
        }
        
        

        private void Set()
        {
            _ = this;
            _locales = Resources.LoadAll<Locale>("NooboLocalize/Locales").ToList();
            tables = Resources.LoadAll<LocalizedTable>("NooboLocalize/Tables").ToList();
        }
    
        public static Locale CurrentLocale()
        {
            return _._locales.Find(locale => locale.name == _.selectedLocale);
        }
        
        // TODO: locales should be only created via settings and with predefined names
        // TODO: Show a selectable and editable list of locales in settings

        
        
        public void OnBeforeSerialize()
        {
            tables?.Clear();
            tables = Resources.LoadAll<LocalizedTable>("NooboLocalize/Tables").ToList();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
