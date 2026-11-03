using Core.Features.Economy.UI;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Logic;
using Core.Infrastructure.AssetManager.Services;
using UnityEngine;
using Zenject;

namespace Core.Features.Economy.Factory
{
    public class InventoryUIFactory : IFactory<InventoryUI>
    {
        private readonly AddressableContainer _addressableContainer;
        
        public InventoryUIFactory(AddressableContainer addressableContainer)
        {
            _addressableContainer = addressableContainer;
        }
        
        public InventoryUI Create()
        {
            var uiPrefab = AddressableContainerExtensions.GetViewPrefab<InventoryUI>(_addressableContainer,UiElements.InventoryUI);
            var inventoryUI = Object.Instantiate(uiPrefab);
            return inventoryUI;
        }
    }
}