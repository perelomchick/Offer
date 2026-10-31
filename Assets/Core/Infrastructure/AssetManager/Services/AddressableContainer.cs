using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Infrastructure.AssetManager.Data;
using Cysharp.Threading.Tasks;
namespace Core.Infrastructure.AssetManager.Services
{
    public class AddressableContainer : IDisposable
    {
        private readonly Dictionary<Type, IItemContainer> _containers = new();

        public void Register<TKey, TValue>(string path)
            where TKey : struct, Enum
            where TValue : UnityEngine.Object
        {
            _containers[typeof(TKey)] = new ItemContainer<TKey, TValue>(path);
        }

        public TValue Get<TKey, TValue>(TKey key)
            where TKey : struct, Enum
            where TValue : UnityEngine.Object
        {
            if (_containers.TryGetValue(typeof(TKey), out var c))
            {
                return ((ItemContainer<TKey, TValue>)c).Get(key);
            }
            
            throw new Exception($"[ERROR]: No container registered for {typeof(TKey).Name}");
        }

        public async UniTask LoadAll(IProgress<float> progress = null, CancellationToken ct = default)
        {
            int total = _containers.Values.Sum(item => item.TotalCount);
            int loaded = 0;

            foreach (var container in _containers.Values)
            {
                float progressCount = ++loaded / (float)total;
                await container.Load(() => progress?.Report(progressCount), ct);
            }
        }

        public void Dispose()
        {
            foreach (var item in _containers.Values)
            {
                item.Dispose();
            }
            
            _containers.Clear();
        }
    }
}