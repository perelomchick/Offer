using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using OfferCore.Data;
using R3;
using UnityEngine;

namespace OfferCore.Common
{
    public class AddressableContainer : IDisposable
    {
        private const string PATH_TO_ADDRESS_RESOURECES = "ResourcesImage/";
        private const string PATH_TO_VIEW_PREFABS = "ViewPrefab/";

        private Dictionary<TypeResource,Sprite> _resourceSprites ;
        private Dictionary<ViewEnum,GameObject> _viewPrefabs;


        public Sprite GetSpriteResource(TypeResource type)  {
            if (_resourceSprites.TryGetValue(type, out Sprite prefab))
                return prefab;
            
            throw new Exception($"Sprite by {type} not found");
        }
        
        public TPrefab GetViewPrefab<TPrefab>(ViewEnum name)
        {
            //Как таковой можно тип TPrefab на премую не указывать, а сделать Dictionary с typeof.
            //Но муторно превращать enum в тип. Тем немение можно
            if (!_viewPrefabs.TryGetValue(name, out GameObject gameObject))
                throw new Exception($"The view -  {name}. Is not loaded");
            
            if(!gameObject.TryGetComponent<TPrefab>(out TPrefab prefab))
                throw new Exception($"View -  {name}. Is not contained on GameObject");

            return prefab;
        }
        
        public async UniTask<Unit> LoadPrefabs(Action onLoaded = null,Action <float> onProgress = null)
        {
            _resourceSprites = await LoadAddressableByEnum<TypeResource, Sprite>(PATH_TO_ADDRESS_RESOURECES, 2, onProgress);
            _viewPrefabs = await LoadAddressableByEnum<ViewEnum, GameObject>(PATH_TO_VIEW_PREFABS, 2, onProgress);
           
            onLoaded?.Invoke();
            return default;
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

        private async Task<Dictionary<TKey, TValue>> LoadAddressableByEnum<TKey, TValue>(
            string path,
            int countOperation,
            Action<float> onProgress = null
            ) where TValue : UnityEngine.Object
        {
            var container = new Dictionary<TKey, TValue>();
            var enumKeys = (TKey[])System.Enum.GetValues(typeof(TKey));
            var numbersLoading = countOperation * enumKeys.Length;
            
            foreach (var enumKey in enumKeys)
            {
                var value = await AddressableLoader
                    .LoadAssetByAddress<TValue>(
                        path + enumKey,
                        onProgress: x => onProgress?.Invoke(x / numbersLoading)
                    );

                container[enumKey] = value;
            }
            
            return container;
        }
    }
}