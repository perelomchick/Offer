using System;
using Core.Features.TimeClock.Data;
using UnityEngine;

namespace Core.Features.Events.Data
{
    [CreateAssetMenu(menuName = "Event Config/Event Config")]
    public class EventData : ScriptableObject
    {
        public int ID;
        public SerializableDateTimeOffset TimeStartEvent;
        public SerializableDateTimeOffset TimeEndEvent; 
        public OfferData[] Offers;
    }
}