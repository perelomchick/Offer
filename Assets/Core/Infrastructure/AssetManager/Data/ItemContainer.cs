using System;
using System.Collections.Generic;
using System.Threading;
using Core.Infrastructure.AssetManager.Logic;
using Cysharp.Threading.Tasks;

namespace Core.Infrastructure.AssetManager.Data
{
    public class ItemContainer<TKey,TValue> : IItemContainer
    where TKey : struct, Enum
    where TValue : UnityEngine.Object
    {
        private string _pathToAssetFolder;
        private Dictionary<TKey,TValue> _items;
        
        public int TotalCount => Enum.GetValues(typeof(TKey)).Length;
        
        public ItemContainer(string pathToAssetFolder)
        {
            _pathToAssetFolder = pathToAssetFolder;
            _items = new Dictionary<TKey,TValue>();
        }

        public TValue Get(TKey key)
        {
            if (_items.TryGetValue(key, out TValue result))
            {
                return result;
            }
            
            throw new Exception($"[ERROR]: Item {{typeof(TValue).Name}} by {{key}} not loaded");
        }

        public async UniTask Load(Action OnLoaded, CancellationToken ct)
        {
            foreach (TKey key in Enum.GetValues(typeof(TKey)))
            {
                ct.ThrowIfCancellationRequested();
                _items[key] = await AddressableLoader
                    .LoadAssetByAddress<TValue>(_pathToAssetFolder + key);
                OnLoaded?.Invoke();
            }
        }

        public void Dispose()
        {
            foreach (var item in _items.Values)
            {
                AddressableLoader.ReleaseAsset(item);
            }
            
            _items.Clear();
        }
    }
}