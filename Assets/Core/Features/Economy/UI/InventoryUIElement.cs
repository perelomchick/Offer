using System;
using Core.Features.Economy.ResourceData;

namespace Core.Features.Economy.UI
{
    [Serializable]
    public class InventoryUIElement
    {
        public ResourceUI ResourceUI;
        public TypeResource Type;
    }
}