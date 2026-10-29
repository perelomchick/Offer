using Core.Features.TimeClock.Data;
using UnityEngine;

namespace Core.Features.Events.Data
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