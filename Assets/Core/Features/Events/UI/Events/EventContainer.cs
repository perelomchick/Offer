using UnityEngine;

namespace Core.Features.Events.UI.Events
{
    public class EventContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _eventContainer;
        
        public Transform GetContainer() => _eventContainer.transform;
    }
}