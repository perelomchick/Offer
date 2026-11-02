using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core.Features.Economy.ResourceData;
using Core.Features.Economy.UI;
using Core.Features.Events.Logic;

namespace Core.Features.Events.UI.Offers
{
    public class OfferUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rewardsContainer;
        [SerializeField] private RectTransform _purchasedIcon;
        [SerializeField] private OnjectGridLayout _rewardGridLayout;
        
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private ResourceUI _resourceUI;

        public Observable<Unit> OnClickBuyButton => _buyButton.OnClickAsObservable();
        
        public void SetPurchasedIcon(bool  isPurchased)
        {
            _purchasedIcon.gameObject.SetActive(isPurchased);
        }

        public void SetBuyInteractable(bool interactable)
        {
            _buyButton.interactable = interactable;
        }

        public void SetGridLayout(List<RectTransform> transforms)
        {
            _rewardGridLayout.SetItems(transforms);
        }

        public void SetDescription(string description)
        {
            _descriptionText.text = description;
        }

        public void SetPriceResource(Resource resource)
        {
           _resourceUI.SetAmount(resource.Amount);
        }

        public Transform GetContainer() =>
            _rewardsContainer;
    }
}  
