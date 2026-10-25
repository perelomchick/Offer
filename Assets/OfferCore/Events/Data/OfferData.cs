using System;
using OfferCore.Data;
using UnityEngine.Serialization;

namespace OfferCore.Events
{
    [Serializable]
    public class OfferData
    {
        public int ID;
        public string Description = "Standard Description";
        public Resource Price = new Resource();
        public Resource[] Rewards;  
    }
}