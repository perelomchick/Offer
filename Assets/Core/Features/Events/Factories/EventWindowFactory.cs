using Core.Features.Events.Logic;
using Core.Features.Events.Services;
using Core.Features.Events.View.Events;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using Zenject;

namespace Core.Features.Events.Factories
{
    public class EventWindowFactory : IFactory<EventWindowPresenter>
    {
        private readonly AddressableContainer _addressables;
        private readonly PurchaseService _purchases;

        public EventWindowFactory(AddressableContainer addressables, PurchaseService purchases)
        {
            _addressables = addressables;
            _purchases = purchases;
        }

        public EventWindowPresenter Create()
        {
            var prefab = _addressables.GetViewPrefab<EventWindowUI>(UiElements.EventWindowUI);
            var ui = UnityEngine.Object.Instantiate(prefab);
            return new EventWindowPresenter(ui, _addressables, _purchases);
        }
    }
}