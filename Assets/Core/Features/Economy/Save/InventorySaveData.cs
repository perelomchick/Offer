using UnityEngine;
using System.Collections.Generic;
using Core.Infrastructure.SaveLoad.Data;
using Core.Features.Economy.ResourceData;

namespace Core.Features.Economy.Save
{
    public class InventorySaveData : ISaveData
    {
        public List<Resource> Resources = new();

        public void SetResource(List<Resource> resource)
        {
            Resources = resource;
        }
        
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}