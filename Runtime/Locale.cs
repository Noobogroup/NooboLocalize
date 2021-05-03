using System;
using TMPro;
using UnityEngine;

namespace NooboPackage.NooboLocalize
{
    [CreateAssetMenu(fileName = "Locale", menuName = "NooboPackage/Localization/New Locale")]
    [Serializable]
    public class Locale : ScriptableObject
    {
        public bool isRtl;
        public TMP_FontAsset customTMPFont;
        [Space(10)]
        public TextAlignmentOptions align;

        public bool overrideNumberCharacterSet;
        public int zeroCharacterCode; 
        
        public string GetFormattedNumber(int number)
        {
            return number + "";
        }
    }
}
