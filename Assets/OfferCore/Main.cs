using Cysharp.Threading.Tasks;
using OfferCore.Common;
using OfferCore.Events.Data;
using OfferCore.Events.Services;
using UnityEngine;
using Zenject;
using EventService = OfferCore.Events.Services.EventService;

//Узнать что EventService в Unity по стандарту
namespace OfferCore
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private ResourceUI _resourceUI;
        [SerializeField] private Transform _eventParent;
        
        [SerializeField] private EventData[] eventData;

        private LoadingCurtainView _loadingCurtainView;
        private AddressableContainer _addressableContainer;
        private EventService _eventService;
        private Inventory _inventory;

        [Inject]
        public void Init(AddressableContainer addressableContainer,
            EventService eventService,
            Inventory inventory,
            LoadingCurtainView loadingCurtainView)
        {
            Debug.Log("[Test] Init Main");
            _addressableContainer = addressableContainer;
            _eventService = eventService;
            _inventory = inventory;
            _loadingCurtainView = loadingCurtainView;
            
            StartGame();
            
            _loadingCurtainView.Hide(destroyCancellationToken).Forget();
        }

        private void StartGame()
        {
            _eventService.StartEvent(eventData,_eventParent);
            _resourceUI.Init(_inventory, _addressableContainer);
        }
    }
}