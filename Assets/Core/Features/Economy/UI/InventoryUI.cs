using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Core.Features.Economy.ResourceData;

namespace Core.Features.Economy.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryUIElement[] _invenroInventoryUIElements;

        private Dictionary<TypeResource, ResourceUI> _resourceByType;

        private Dictionary<TypeResource, ResourceUI> ResourceByType
        {
            get
            {
                return _resourceByType ??= _invenroInventoryUIElements
                    .ToDictionary(x => x.Type, x => x.ResourceUI);
            }
        }


        public void SetSprite(TypeResource typeResource, Sprite sprite)
        {
            ResourceByType[typeResource].SetSprite(sprite);
        }

        public void SetAmount(TypeResource typeResource, int amount)
        {
            ResourceByType[typeResource].SetAmount(amount);
        }
        
        public TypeResource[] GetElementTypes()
        {
            return _invenroInventoryUIElements.Select(x => x.Type).ToArray();
        }
    }
}