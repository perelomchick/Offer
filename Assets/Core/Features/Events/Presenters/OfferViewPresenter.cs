using R3;
using System;
using UnityEngine;
using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Services;
using Core.Features.Events.View.Offers;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;

namespace Core.Features.Events.Presenters
{
    public class OfferViewPresenter : IDisposable
    {
        private readonly OfferView _offerView;
        private readonly AddressableContainer _addressableContainer;
        private readonly PurchaseService _purchaseService;
        private readonly ViewCollection<ResourceView> _resources;
        private readonly IDisposable _buySubscription;
        
        private EventData _eventData;
        private OfferData _offerData;
        
        public OfferViewPresenter(OfferView offerView,
            AddressableContainer addressableContainer,
            PurchaseService purchaseService)
        {
            _offerView = offerView;
            _addressableContainer = addressableContainer;
            _purchaseService = purchaseService;

            var resourceViewPrefab = _addressableContainer.GetViewPrefab<ResourceView>(UiElements.ResourceView);
            _resources = new ViewCollection<ResourceView>(resourceViewPrefab, _offerView.GetContainer());

            _buySubscription = _offerView
                .OnClickBuyButton.Subscribe(_ => OnBuyClicked())
                .AddTo(_offerView);
        }

        public void Dispose() => DisposeAll();
        


        public void SetData(EventData eventData, OfferData offerData)
        {
            _eventData = eventData;
            _offerData = offerData;

            _offerView.SetDescription(offerData.Description);
            _offerView.GetPriceResourceView().Init(_addressableContainer, offerData.Price);
            _offerView.GetPriceResourceView().SetAmount(offerData.Price.Amount);

            var result = _resources.Fill(offerData.Rewards,
                (view, data) => view.Init(_addressableContainer, data));
            
            _offerView.SetGridLayout(result.ConvertAll(i => (RectTransform)i.transform));

            Refresh();
        }

        private void OnBuyClicked()
        {
            if (_purchaseService.TryBuy(_eventData, _offerData))
                Refresh();
        }

        private void Refresh()
        {
            var isPurchased = _purchaseService.IsPurchased(_eventData, _offerData);
            _offerView.SetPurchasedIcon(isPurchased);
            _offerView.SetBuyInteractable(!isPurchased && _purchaseService.CanBuy(_eventData, _offerData));
        }

        private void DisposeAll()
        {
            _buySubscription.Dispose();
            _resources.Dispose();
        }
    }
}