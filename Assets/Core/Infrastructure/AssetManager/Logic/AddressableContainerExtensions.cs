using System;
using UnityEngine;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;

namespace Core.Infrastructure.AssetManager.Logic
{
    public static class AddressableContainerExtensions
    {
        public static TPrefab GetViewPrefab<TPrefab>(this AddressableContainer c, UiElements name)
        {
            var go = c.Get<UiElements, GameObject>(name);
            if (!go.TryGetComponent<TPrefab>(out var prefab))
                throw new Exception($"[ERROR] View - {name}. Is not contained on GameObject");
            return prefab;
        }
    }
}