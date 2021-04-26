using NooboPackage.NooboLocalize.Runtime.ImageTable;
using NooboPackage.NooboLocalize.Runtime.TextTable;
using UnityEditor;
using UnityEngine;

namespace NooboPackage.NooboLocalize.Editor
{
    public class ContextMenuExtension : UnityEditor.Editor
    {
        [MenuItem("CONTEXT/TMP_Text/Noobo Localize")]
        public static void TMPLocalize(MenuCommand command)
        {
            var obj = (command.context as Component)?.gameObject;
            if(obj == null)
                return;

            if (obj.GetComponent<LocalizedTMPText>() == null)
                obj.AddComponent<LocalizedTMPText>();
        }
        
        [MenuItem("CONTEXT/Image/Noobo Localize")]
        public static void ImageLocalize(MenuCommand command)
        {
            var obj = (command.context as Component)?.gameObject;
            if(obj == null)
                return;

            if (obj.GetComponent<LocalizedUIImage>() == null)
                obj.AddComponent<LocalizedUIImage>();
        }
    }
}