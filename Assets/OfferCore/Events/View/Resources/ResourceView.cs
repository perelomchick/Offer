using OfferCore.Common;
using OfferCore.Data;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

namespace OfferCore.Events.View.Resources
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] public TypeResource TypeResource;
        
        [SerializeField] private Image _iconResource;
        [SerializeField] private TextMeshProUGUI _amountResource;
        
        public void SetAmount(int amount) =>
            _amountResource.text = amount.ToString();
        
        private void SetSprite(Sprite sprite) =>
            _iconResource.sprite = sprite;
        
        public void Init(AddressableContainer addressableContainer,Resource resource = null)
        {
            if (resource != null)
            {
                SetAmount(resource.Amount);
                TypeResource = resource.Type;
            }
            
            SetSprite(addressableContainer.GetSpriteResource(TypeResource));
        }
    }
}