using Core.Data;
using UnityEngine;

namespace Core.Events.Data
{
    [CreateAssetMenu(menuName = "Event Config/Event Config")]
    public class EventData : ScriptableObject
    {
        public int ID;
        public TimeDate TimeStartEvent;
        public TimeDate TimeEndEvent; 
        public OfferData[] Offers;
    }
}