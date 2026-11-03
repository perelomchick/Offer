using R3;
using UnityEngine;
using Core.Features.Economy.UI;
using Core.Features.Economy.Logic;
using Core.Features.Economy.ResourceData;
using Core.Infrastructure.AssetManager.Services;

namespace Core.Features.Economy.Presenters
{
    public class InventoryUIPresenter
    {
        public InventoryUIPresenter(InventoryUI inventoryUI, Inventory inventory, AddressableContainer addressableContainer)
        {
            foreach (var type in inventoryUI.GetElementTypes())
            { 
                var sprite = addressableContainer.Get<TypeResource,Sprite>(type);
                inventoryUI.SetSprite(type, sprite);
                inventory.GetObservableResource(type)
                    .Subscribe(x => inventoryUI.SetAmount(type,x))
                    .AddTo(inventoryUI.gameObject);
            }
        }
    }
}