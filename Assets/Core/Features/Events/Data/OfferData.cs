using System;
using Core.Features.Economy.ResourceData;

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