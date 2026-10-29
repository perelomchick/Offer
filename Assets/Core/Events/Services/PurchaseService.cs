using System;
using System.Linq;
using Core.Events.Data;

namespace Core.Events.Services
{
    public class PurchaseService
    { 
        private readonly SaveService _saveService;
        private readonly Inventory _inventory;
        private readonly SaveData _saveData;
    
        public event Action<EventData> EventCompleted;
    
        public PurchaseService(SaveService saveService, Inventory inventory)
        {
            _saveService = saveService;
            _inventory = inventory;
            _saveData = saveService.Load();
    
            if (_saveData.Resources.Count > 0)
                _inventory.LoadFromSave(_saveData.Resources);
        }
    
        public bool IsPurchased(EventData eventData, OfferData offer) =>
            _saveData.PurchasedOffers.Contains(MakeKey(eventData, offer));
    
        public bool IsEventCompleted(EventData eventData) =>
            eventData.Offers.Length > 0 &&
            eventData.Offers.All(offer => IsPurchased(eventData, offer));
    
        public bool CanBuy(EventData eventData, OfferData offer) =>
            !IsPurchased(eventData, offer)
            && _inventory.GetAmount(offer.Price.Type) >= offer.Price.Amount;
    
        private static string MakeKey(EventData eventData, OfferData offer) =>
            $"{eventData.ID}:{offer.ID}";
        
        public bool TryBuy(EventData eventData, OfferData offer)
        {
            if (!CanBuy(eventData, offer))
                return false;
    
            _inventory.RemoveResource(offer.Price.Type, offer.Price.Amount);
    
            foreach (var reward in offer.Rewards)
                _inventory.AddResource(reward.Type, reward.Amount);
    
            _saveData.PurchasedOffers.Add(MakeKey(eventData, offer));
            SaveProgress();
    
            if (IsEventCompleted(eventData))
                EventCompleted?.Invoke(eventData);
    
            return true;
        }
    
        private void SaveProgress()
        {
            _saveData.Resources = _inventory.ToSaveData();
            _saveService.Save(_saveData);
        }
    }
}