using System;
using System.Collections.Generic;
using OfferCore.Data;

namespace OfferCore.Events.Data
{
    [Serializable]
    public class SaveData
    {
        public List<string> PurchasedOffers = new();
        public List<Resource> Resources = new();
    }
}