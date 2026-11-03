using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.Economy.UI
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private Image _iconResource;
        [SerializeField] private TextMeshProUGUI _amountResource;
        
        public void SetAmount(int amount) =>
            _amountResource.text = amount.ToString();
        
        public void SetSprite(Sprite sprite) =>
            _iconResource.sprite = sprite;
    }
}