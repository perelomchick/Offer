using OfferCore.Common;
using OfferCore.Events.Data;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OfferCore.Events.View.Events
{
    public class EventView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _openOfferWindowButton;
        
        public Observable<Unit> OpenWindow => _openOfferWindowButton.onClick.AsObservable();
        
        public void SetTimerText(string text) => _timerText.text = text;
    }
}
