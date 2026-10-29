using Core.Common;
using Core.Data;
using Core.Events.Services;
using Core.Events.View.Resources;
using R3;
using UnityEngine;

namespace Core
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