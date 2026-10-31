using System;
using System.Collections.Generic;
using Core.Infrastructure.SaveLoad.Data;
using UnityEngine;

namespace Core.Features.Events.Save
{
    [Serializable]
    public class EventSaveData : ISaveData
    {
        public List<Vector2Int> PirchesedOffers = new();
        
        public bool ContainsPurchasedOffer(int eventId,int offerId)
        {
            foreach (var item in PirchesedOffers)
            {
                if(item.x == eventId && item.y == offerId)
                    return true;
            }
            
            return false;
        }

        public void AddData(int eventID, int offerID)
        { 
            if (!ContainsPurchasedOffer(eventID, offerID))
            { 
                PirchesedOffers.Add(new Vector2Int(eventID, offerID));
                return;
            }

            Debug.LogWarning($"Event {eventID} has been added to offer {offerID}");
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}