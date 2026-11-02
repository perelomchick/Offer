using R3;
using System;
using UnityEngine;
using System.Collections.Generic;
using Core.Features.Economy.UI;
using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Services;
using Core.Features.Economy.Presenters;
using Core.Features.Events.UI.Offers;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Logic;
using Core.Infrastructure.AssetManager.Services;

namespace Core.Features.Events.Presenters
{
    public class OfferViewPresenter : IDisposable
    {
        private readonly OfferUI _offerUI;
        private readonly AddressableContainer _addressableContainer;
        private readonly PurchaseService _purchaseService;
        
        private readonly ViewCollection<ResourceUI> _resources;
        private readonly Dictionary<ResourceUI, RewardResourcePresenter> _resourceUIPresenters = new();
        
        private readonly IDisposable _buySubscription;
        
        private EventData _eventData;
        private OfferData _offerData;
        
        public OfferViewPresenter(
            OfferUI offerUI,
            AddressableContainer addressableContainer,
            PurchaseService purchaseService)
        {
            _offerUI = offerUI;
            _addressableContainer = addressableContainer;
            _purchaseService = purchaseService;

            var resourceViewPrefab = _addressableContainer.GetViewPrefab<ResourceUI>(UiElements.ResourceView);
            _resources = new ViewCollection<ResourceUI>(resourceViewPrefab, _offerUI.GetContainer());

            _buySubscription = _offerUI
                .OnClickBuyButton.Subscribe(_ => OnBuyClicked())
                .AddTo(_offerUI);
        }

        public void SetData(EventData eventData, OfferData offerData)
        {
            _eventData = eventData;
            _offerData = offerData;

            _offerUI.SetDescription(offerData.Description);
            _offerUI.SetPriceResource(offerData.Price);
            
            var result = _resources.Fill(offerData.Rewards,
                (view, data) => GerResourcePresenter(view).SetData(data));
            
            
            _offerUI.SetGridLayout(result.ConvertAll(i => (RectTransform)i.transform));

            Refresh();
        }

        private RewardResourcePresenter GerResourcePresenter(ResourceUI resourceUI)
        {
            if (!_resourceUIPresenters.TryGetValue(resourceUI, out var presenter))
            {
                presenter = new RewardResourcePresenter(resourceUI, _addressableContainer);
                _resourceUIPresenters.Add(resourceUI, presenter);
            }
            
            return presenter;
        }

        private void OnBuyClicked()
        {
            if (_purchaseService.TryBuy(_eventData, _offerData))
                Refresh();
        }

        private void Refresh()
        {
            var isPurchased = _purchaseService.IsPurchased(_eventData, _offerData);
            _offerUI.SetPurchasedIcon(isPurchased);
            _offerUI.SetBuyInteractable(!isPurchased && _purchaseService.CanBuy(_eventData, _offerData));
        }

        public void Dispose()
        {
            _buySubscription.Dispose();
            _resources.Dispose();
        }
    }
}