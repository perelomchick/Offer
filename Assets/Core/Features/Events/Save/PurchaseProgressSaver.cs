using System;
using Core.App.Data;
using Core.Features.Events.Services;
using Core.Infrastructure.SaveLoad.Services;
using R3;

namespace Core.Features.Events.Save
{
    public class PurchaseProgressSaver : IDisposable
    {
        private readonly PurchaseService _purchaseService;
        private readonly SaveService _saveService;
        private readonly IDisposable _disposable;

        public PurchaseProgressSaver(PurchaseService purchaseService, SaveService saveService)
        {
            _purchaseService = purchaseService;
            _saveService = saveService;
            _disposable = _purchaseService.OfferPurchased.Subscribe(_ => SaveData());

        }
        
        public void Dispose()
        {
            _disposable?.Dispose();
        }
        
        private void SaveData()
        {
            _saveService.Save(ConstName.NAME_FOLDER_SAVE, ConstName.NAME_EVENT_SAVE_FILE, _purchaseService.GetSaveData());
        }
    }
}