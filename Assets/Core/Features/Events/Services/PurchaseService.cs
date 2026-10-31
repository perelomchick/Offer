using System;
using Core.Features.Economy.Logic;
using Core.Features.Events.Data;
using Core.Features.Events.Save;
using Core.Infrastructure.SaveLoad.Data;
using R3;

namespace Core.Features.Events.Services
{
    public class PurchaseService : IDisposable
    { 
        private readonly Inventory _inventory;
        private readonly Subject<Unit> _offerPurchased = new();
        private readonly EventSaveData _eventSaveData;
        
        public Observable<Unit> OfferPurchased => _offerPurchased;
    
        public PurchaseService(Inventory inventory,EventSaveData eventSaveData)
        {
            _inventory = inventory;
            _eventSaveData = eventSaveData;
        }
        
        public bool IsPurchased(EventData eventData, OfferData offer)
        {
            return _eventSaveData.ContainsPurchasedOffer(eventData.ID, offer.ID);
        }
    
        public bool CanBuy(EventData eventData, OfferData offer)
        {
            var enoughResource = _inventory.GetAmount(offer.Price.Type) >= offer.Price.Amount;
            return !IsPurchased(eventData, offer) && enoughResource;
        }
        
        public ISaveData GetSaveData()
        {
            return _eventSaveData;
        }

        public bool TryBuy(EventData eventData, OfferData offer)
        {
            if (!CanBuy(eventData, offer))
                return false;
    
            _inventory.RemoveResource(offer.Price.Type, offer.Price.Amount);
    
            foreach (var reward in offer.Rewards)
                _inventory.AddResource(reward.Type, reward.Amount);
    
            _eventSaveData.AddData(eventData.ID, offer.ID);
            _offerPurchased.OnNext(Unit.Default);
            
            return true;
        }

        public void Dispose()
        {
            _offerPurchased?.Dispose();
        }
    }
}