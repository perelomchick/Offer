using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.Events.UI.Events
{
    public class EventUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _openOfferWindowButton;
        
        public Observable<Unit> OpenWindow => _openOfferWindowButton.onClick.AsObservable();
        
        public void SetTimerText(string text) => _timerText.text = text;
    }
}
