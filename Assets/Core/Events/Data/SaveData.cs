using System;
using System.Collections.Generic;
using Core.Data;

namespace Core.Events.Data
{
    [Serializable]
    public class SaveData
    {
        public List<string> PurchasedOffers = new();
        public List<Resource> Resources = new();
    }
}