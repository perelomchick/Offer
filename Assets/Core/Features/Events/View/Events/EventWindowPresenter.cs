using System;
using System.Collections.Generic;
using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Services;
using Core.Features.Events.View.Offers;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using R3;

namespace Core.Features.Events.View.Events
{
    public class EventWindowPresenter : IDisposable
    {
        private readonly EventWindowUI _eventWindowUI;
        private readonly ViewCollection<OfferView> _offersCollection;
        private readonly Dictionary<OfferView, OfferViewPresenter> _offerViewPresenters = new();
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

            var offerViewPrefab = _addressableContainer.GetViewPrefab<OfferView>(UiElements.OfferView);
            _offersCollection = new ViewCollection<OfferView>(offerViewPrefab, _eventWindowUI.GetRootTransform());
        }

        public void Dispose() => DisposeAll();
        
        public void SetEventData(EventData eventData)
        {
            _eventWindowUI.Show();
            _offersCollection.Fill(eventData.Offers,
                (view, data) => GetOfferViewPresenter(view).SetData(eventData, data));
        }

        private OfferViewPresenter GetOfferViewPresenter(OfferView offerView)
        {
            if (!_offerViewPresenters.TryGetValue(offerView, out var presenter))
            {
                presenter = new OfferViewPresenter(offerView, _addressableContainer, _purchaseService);
                _offerViewPresenters.Add(offerView, presenter);
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