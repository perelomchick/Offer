using System;
using System.Collections.Generic;
using System.Threading;
using Core.Data;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Infrastructure.AssetManager.Services
{
    public class AddressableContainer : IDisposable
    {
        private const string PATH_TO_ADDRESS_RESOURECES = "ResourcesImage/";
        private const string PATH_TO_VIEW_PREFABS = "ViewPrefab/";

        private Dictionary<TypeResource,Sprite> _resourceSprites = new();
        private Dictionary<UiElements,GameObject> _viewPrefabs = new();
        
        private int _loadedCount;
        private int _totalCount;

        public Sprite GetSpriteResource(TypeResource type)  {
            if (_resourceSprites.TryGetValue(type, out Sprite prefab))
                return prefab;
            
            throw new Exception($"Sprite by {type} not found");
        }
        
        public TPrefab GetViewPrefab<TPrefab>(UiElements name)
        {
            //Как таковой можно тип TPrefab на премую не указывать, а сделать Dictionary с typeof.
            //Но муторно превращать enum в тип. Тем немение можно
            if (!_viewPrefabs.TryGetValue(name, out GameObject gameObject))
                throw new Exception($"[ERROR] The view -  {name}. Is not loaded");
            
            if(!gameObject.TryGetComponent<TPrefab>(out TPrefab prefab))
                throw new Exception($"[ERROR] View - {name}. Is not contained on GameObject");

            return prefab;
        }
        
        public async UniTask LoadPrefabsAsync(IProgress<float> progress = null, CancellationToken ct = default)
        {
            _loadedCount = 0;
            _totalCount = Enum.GetValues(typeof(TypeResource)).Length
                          + Enum.GetValues(typeof(UiElements)).Length;

            _resourceSprites = await LoadAddressableByEnum<TypeResource, Sprite>(
                PATH_TO_ADDRESS_RESOURECES, progress, ct);

            _viewPrefabs = await LoadAddressableByEnum<UiElements, GameObject>(
                PATH_TO_VIEW_PREFABS, progress, ct);
        }

        public void Dispose()
        {
            foreach (var resourceSprite in _resourceSprites.Values)
                AddressableLoader.ReleaseAsset(resourceSprite);
            
            foreach (var viewPrefab in _viewPrefabs.Values)
                AddressableLoader.ReleaseAsset(viewPrefab);
            
            _resourceSprites.Clear();
            _viewPrefabs.Clear();
        }

        private async UniTask<Dictionary<TKey, TValue>> LoadAddressableByEnum<TKey, TValue>(
            string path,
            IProgress<float> progress,
            CancellationToken ct) where TValue : UnityEngine.Object
        {
            var container = new Dictionary<TKey, TValue>();
            var enumKeys = (TKey[])Enum.GetValues(typeof(TKey));

            foreach (var enumKey in enumKeys)
            {
                ct.ThrowIfCancellationRequested();

                container[enumKey] = await AddressableLoader.LoadAssetByAddress<TValue>(
                    path + enumKey,
                    onProgress: x => progress?.Report((_loadedCount + x) / _totalCount));

                _loadedCount++;
                progress?.Report(_loadedCount / (float)_totalCount);
            }

            return container;
        }
    }
}