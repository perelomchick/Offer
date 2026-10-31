using System;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using UnityEngine;

namespace Core.Features.Events.Logic
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