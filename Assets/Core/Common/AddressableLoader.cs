using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Common
{
    public static class AddressableLoader
    {
        public static async Task<T> LoadClassByAddress<T>(string address,
        Action<T> onCompleted = null,
        Action<float> onProgress = null)
        where T : UnityEngine.Object
        {
            var gameObject = await LoadAssetByAddress<GameObject>(address,null,onProgress);

            if (!gameObject.TryGetComponent(out T result))
            {
                Debug.LogError($"[AddressableLoader] {nameof(T)} don`t contented class");
                return null;
            }
            
            onCompleted?.Invoke(result);
            return result;
        }

        public static async Task<T> LoadAssetByAddress<T>(string address,
            Action<T> onCompleted = null,
            Action<float> onProgress = null)
        where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogError($"[AddressableLoader] {nameof(address)} address is null or empty");
                return null;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return await WaitForHandle(handle, address, onCompleted, onProgress);
        }
        
        public static async Task<IList<T>> LoadAssetsByLabel<T>(string label,
            Action<IList<T>> onCompleted = null,
            Action<float> onProgress = null)    
        {
            if (string.IsNullOrEmpty(label))
            {
                Debug.LogError($"[AddressableLoader] {nameof(label)} label is null or empty");
                return null;
            }

            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            return await WaitForHandle(handle, label,onCompleted, onProgress);
        }
        
        public static void ReleaseAsset<T>(T asset)
        {
            if (asset != null)
            {
                Addressables.Release(asset);
            }
        }

        private static async Task<T> WaitForHandle<T>(AsyncOperationHandle<T> handle,string key, 
            Action<T> onCompleted = null,
            Action<float> onProgress = null)
        {
            while (!handle.IsDone)
            {
                onProgress?.Invoke(handle.PercentComplete);
                await Task.Yield();
            }
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onProgress?.Invoke(1f);
                onCompleted?.Invoke(handle.Result);
                return handle.Result;
            }

            Debug.LogError($"[AddressableLoader] Не удалось загрузить ассет по ключу: {key}. Ошибка: {handle.OperationException}");
            
            if(handle.IsValid())
                Addressables.Release(handle);
            
            return default;
        }
    }
}