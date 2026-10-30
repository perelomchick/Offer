using Core.Data;
using Core.Infrastructure.AssetManager.Services;
using Core.Resources;
using R3;
using UnityEngine;

namespace Core.Features.Economy.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private ResourceView[] allResources;

        public void Init(Inventory inventory,AddressableContainer  addressableContainer)
        {
            foreach (var resource in allResources)
            {
                resource.Init(addressableContainer);
                SubscribeToResources(inventory, resource.TypeResource, resource);
            }
        }

        private void SubscribeToResources(Inventory inventory,TypeResource typeResource,ResourceView resourceView)
        {
            inventory.GetObservableResource(typeResource)
                .Subscribe(resourceView.SetAmount);
        }
    }
}