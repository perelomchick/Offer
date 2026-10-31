using System.Collections.Generic;
using Core.Features.Economy.ResourceData;
using Core.Infrastructure.SaveLoad.Data;
using UnityEngine;

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