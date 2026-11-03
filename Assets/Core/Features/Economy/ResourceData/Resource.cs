using System;

namespace Core.Features.Economy.ResourceData
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