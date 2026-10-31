using Core.Features.Economy.Logic;
using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using EventService = Core.Features.Events.Services.EventService;

//Узнать что EventService в Unity по стандарту
namespace Core.App
{
    public class GameBootstrap : IInitializable
    { 
        private EventData[] _eventData;
        
        private InventoryUI _inventoryUI;
        private LoadingCurtainUI _loadingCurtainUI;
        private AddressableContainer _addressableContainer;
        private EventService _eventService;
        private Inventory _inventory;

        public GameBootstrap (AddressableContainer addressableContainer,
            EventService eventService,
            Inventory inventory,
            LoadingCurtainUI loadingCurtainUI,
            EventData[] eventData)
        {
            _addressableContainer = addressableContainer;
            _eventService = eventService;
            _inventory = inventory;
            _loadingCurtainUI = loadingCurtainUI;
            _eventData = eventData;
        }
        
        public void Initialize()
        {
            Debug.Log("startGame");
            StartGame();
            _loadingCurtainUI.Hide().Forget();
        }

        private void StartGame()
        {
            var ui = AddressableContainerExtensions.GetViewPrefab<InventoryUI>(_addressableContainer,UiElements.InventoryUI);
            _inventoryUI = Object.Instantiate(ui);
            _eventService.StartEvent(_eventData);
            _inventoryUI.Init(_inventory, _addressableContainer);
        }


    }
}