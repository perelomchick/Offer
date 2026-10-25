using System;

namespace OfferCore.Data
{
    [Serializable]
    public class Resource
    {
        public TypeResource Type;
        public int Amount;

        public Resource()
        {
            Type = TypeResource.Diamond;
            Amount = 0;
        }
    }
}