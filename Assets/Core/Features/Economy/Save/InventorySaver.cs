using System;
using Core.App.Data;
using Core.Features.Economy.Logic;
using Core.Infrastructure.SaveLoad.Services;
using R3;

namespace Core.Features.Economy.Save
{
    public class InventorySaver : IDisposable
    {
        private readonly Inventory _inventory;
        private readonly SaveService _saveService;
        private readonly IDisposable _disChanged;

        public InventorySaver(Inventory inventory, SaveService saveService)
        {
            _inventory = inventory;
            _saveService = saveService;

            _disChanged = _inventory.Changed.Subscribe(_ => Save());
        }
        
        public void Dispose()
        {
            _disChanged.Dispose();
        }
        
        private void Save()
        {
            _saveService.Save(ConstName.NAME_FOLDER_SAVE,ConstName.NAME_INVENTORY_SAVE_FILE,_inventory.ToSaveData());
        }
    }
}