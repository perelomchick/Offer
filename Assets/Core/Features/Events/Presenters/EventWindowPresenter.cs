using System;
using System.Collections.Generic;
using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Services;
using Core.Features.Events.UI.Events;
using Core.Features.Events.UI.Offers;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Logic;
using Core.Infrastructure.AssetManager.Services;
using R3;

namespace Core.Features.Events.Presenters
{
    public class EventWindowPresenter : IDisposable
    {
        private readonly EventWindowUI _eventWindowUI;
        private readonly ViewCollection<OfferUI> _offersCollection;
        private readonly Dictionary<OfferUI, OfferViewPresenter> _offerViewPresenters = new();
        private readonly AddressableContainer _addressableContainer;
        private readonly PurchaseService _purchaseService;
        private readonly IDisposable _closeSubscription;

        public EventWindowPresenter(EventWindowUI eventWindowUI, AddressableContainer addressableContainer,
            PurchaseService purchaseService)
        {
            _eventWindowUI = eventWindowUI;
            _addressableContainer = addressableContainer;
            _purchaseService = purchaseService;

            _eventWindowUI.Hide();
            _closeSubscription = _eventWindowUI.CloseWindow.Subscribe(_ => _eventWindowUI.Hide());

            var offerViewPrefab = _addressableContainer.GetViewPrefab<OfferUI>(UiElements.OfferView);
            _offersCollection = new ViewCollection<OfferUI>(offerViewPrefab, _eventWindowUI.GetRootTransform());
        }

        public void Dispose() => DisposeAll();
        
        public void SetEventData(EventData eventData)
        {
            _eventWindowUI.Show();
            _offersCollection.Fill(eventData.Offers,
                (view, data) => GetOfferViewPresenter(view).SetData(eventData, data));
        }

        private OfferViewPresenter GetOfferViewPresenter(OfferUI offerUI)
        {
            if (!_offerViewPresenters.TryGetValue(offerUI, out var presenter))
            {
                presenter = new OfferViewPresenter(offerUI, _addressableContainer, _purchaseService);
                _offerViewPresenters.Add(offerUI, presenter);
            }
            return presenter;
        }
        
        private void DisposeAll()
        {
            _closeSubscription.Dispose();
            foreach (var presenter in _offerViewPresenters.Values)
                presenter.Dispose();
            _offerViewPresenters.Clear();
        }
    }
}