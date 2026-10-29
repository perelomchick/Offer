using System;
using Core.Data;

namespace Core.Features.Events.Data
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