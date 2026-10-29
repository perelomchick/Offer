using System.Collections.Generic;
using Core.Features.Economy.UI;
using Core.Features.Events.Logic;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.Events.View.Offers
{
    public class OfferView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rewardsContainer;
        [SerializeField] private RectTransform _purchasedIcon;
        [SerializeField] private OnjectGridLayout _rewardGridLayout;
        
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private ResourceView _priceResourceView;

        public Observable<Unit> OnClickBuyButton => _buyButton.OnClickAsObservable();
        
        public void SetPurchasedIcon(bool  isPurchased)=>
            _purchasedIcon.gameObject.SetActive(isPurchased);
        
        public void SetBuyInteractable(bool interactable) =>
            _buyButton.interactable = interactable;
        
        public void SetGridLayout(List<RectTransform> transforms) =>
            _rewardGridLayout.SetItems(transforms);
        
        public void SetDescription(string description)=> 
            _descriptionText.text = description; 

        public ResourceView GetPriceResourceView() => 
            _priceResourceView;
        
        public Transform GetContainer() =>
            _rewardsContainer;
    }
}  
