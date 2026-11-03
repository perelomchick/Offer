using UnityEngine;
using Core.Features.Economy.UI;
using Core.Features.Economy.ResourceData;
using Core.Infrastructure.AssetManager.Services;

namespace Core.Features.Economy.Presenters
{
    public class RewardResourcePresenter
    {
        private readonly ResourceUI _resourceUI;
        private readonly AddressableContainer _addressableContainer;

        public RewardResourcePresenter(
            ResourceUI resourceUI,
            AddressableContainer addressableContainer)
        {
            _resourceUI = resourceUI;
            _addressableContainer =  addressableContainer;
        }

        public void SetData(Resource resource)
        {
            var sprite = _addressableContainer.Get<TypeResource,Sprite>(resource.Type);
            _resourceUI.SetAmount(resource.Amount);
            _resourceUI.SetSprite(sprite);
        }
    }
}