using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Events.View.Events
{
    public class EventWindowUI : MonoBehaviour
    {
        [SerializeField] private GameObject _eventWindowRoot;
        [SerializeField] private Button _closeWindowButton;

        public Transform GetRootTransform() => _eventWindowRoot.transform;
        public Observable<Unit> CloseWindow => _closeWindowButton.onClick.AsObservable();
        
        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);
    }
}