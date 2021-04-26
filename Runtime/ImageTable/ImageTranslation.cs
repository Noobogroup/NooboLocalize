using System;
using System.Collections.Generic;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Runtime.ImageTable
{
    [Serializable]
    public class ImageTranslation : Dictionary<string, Sprite>, ISerializationCallbackReceiver 
    {
        [SerializeField] public List<string> keys;
        [SerializeField] public List<Sprite> values;

        
        public ImageTranslation()
        {
            keys = new List<string>();
            values = new List<Sprite>();
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
    }
}
