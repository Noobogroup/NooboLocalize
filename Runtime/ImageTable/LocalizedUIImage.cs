using UnityEngine;
using UnityEngine.UI;

namespace NooboPackage.NooboLocalize.Runtime.ImageTable
{
    public class LocalizedUIImage : MonoBehaviour
    {
        [SerializeField] public LocalizedImageReference id;

        private Image _targetComponent; 
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
            _targetComponent = GetComponent<Image>();
        }
        

        public void LocalizeUpdate()
        {
            Initialize();
            
            _targetComponent.sprite = LocalizeLoader.GetSprite(id.table, id.key);
        }
    }
}
