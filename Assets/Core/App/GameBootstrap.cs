using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using Core.Infrastructure.AssetManager.Services;
using Core.Resources;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using EventService = Core.Features.Events.Services.EventService;

//Узнать что EventService в Unity по стандарту
namespace Core.App
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private ResourceUI _resourceUI;
        [SerializeField] private Transform _eventParent;
        
        [SerializeField] private EventData[] eventData;

        private LoadingCurtainUI _loadingCurtainUI;
        private AddressableContainer _addressableContainer;
        private EventService _eventService;
        private Inventory _inventory;

        [Inject]
        public void Init(AddressableContainer addressableContainer,
            EventService eventService,
            Inventory inventory,
            LoadingCurtainUI loadingCurtainUI)
        {
            _addressableContainer = addressableContainer;
            _eventService = eventService;
            _inventory = inventory;
            _loadingCurtainUI = loadingCurtainUI;
            
            StartGame();
            
            _loadingCurtainUI.Hide(destroyCancellationToken).Forget();
        }

        private void StartGame()
        {
            _eventService.StartEvent(eventData,_eventParent);
            _resourceUI.Init(_inventory, _addressableContainer);
        }
    }
}