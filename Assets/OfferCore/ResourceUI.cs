using OfferCore.Common;
using OfferCore.Data;
using OfferCore.Events.Services;
using OfferCore.Events.View;
using OfferCore.Events.View.Resources;
using R3;
using UnityEngine;

namespace OfferCore
{
    public class ResourceUI : MonoBehaviour
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