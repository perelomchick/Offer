using Zenject;
using Cysharp.Threading.Tasks;
using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using EventService = Core.Features.Events.Services.EventService;

namespace Core.App
{
    public class GameBootstrap : IInitializable
    { 
        private EventData[] _eventData;
        
        private InventoryUI _inventoryUI;
        private LoadingCurtainUI _loadingCurtainUI;
        private EventService _eventService;

        public GameBootstrap (
            EventService eventService,
            LoadingCurtainUI loadingCurtainUI,
            EventData[] eventData)
        {
            _eventService = eventService;
            _loadingCurtainUI = loadingCurtainUI;
            _eventData = eventData;
        }
        
        public void Initialize()
        {
            StartGame();
        }

        private void StartGame()
        {  
            _loadingCurtainUI.Hide().Forget();
            _eventService.StartEvent(_eventData);
        }
    }
}