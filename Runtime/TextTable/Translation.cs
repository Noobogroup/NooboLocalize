using System;
using System.Collections.Generic;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.TextTable
{
    [Serializable]
    public class Translation : Dictionary<string, string>, ISerializationCallbackReceiver
    {
        [SerializeField] public List<string> keys;
        [SerializeField] public List<string> values;

        public Translation()
        {
            keys = new List<string>();
            values = new List<string>();
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach(var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
 
            if(keys.Count != values.Count)
                throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
 
            for(var i = 0; i < keys.Count; i++)
                this[keys[i]] = values[i];
        }
        
        public static Translation SameForAll(string txt)
        {
            var trans = new Translation();
            foreach (var locale in NooboLocalizeSettings.Locales)
            {
                trans.Add(locale.name, txt);
            }
            return trans;
        }
    }
}
