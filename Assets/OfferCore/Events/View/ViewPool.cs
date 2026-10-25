using UnityEngine;
using UnityEngine.Pool;

namespace OfferCore.Events.View
{
    public class ViewPool<TComponent> where TComponent : Component
    {
        private const int DEFAULT_POOL_SIZE = 4;
        private const int MAX_POOL_SIZE = 10;
        
        private readonly ObjectPool<TComponent> _pool;
        private readonly TComponent _prefab;
        private readonly Transform _poolRoot;

        public ViewPool(TComponent prefab, Transform poolRoot)
        {
            _prefab = prefab;
            _poolRoot = poolRoot;

            _pool = new ObjectPool<TComponent>(
                Create,
                OnGet,
                OnRelease,
                OnDestroy,
                true,
                DEFAULT_POOL_SIZE,
                MAX_POOL_SIZE);
        }
        public void Release(TComponent resourceView) =>
            _pool.Release(resourceView);
        
        public void Clear() =>
            _pool.Clear();
        
        private TComponent Create() =>
            Object.Instantiate<TComponent>(_prefab, _poolRoot);
        
        private void OnGet(TComponent view) 
            => view.gameObject.SetActive(true);

        private void OnDestroy(TComponent view)
        {
            if (view != null)
            {
                Object.Destroy(view.gameObject);
            }
        }

        public TComponent Get()
        {
            var view = _pool.Get();
            view.transform.SetParent(_poolRoot,false);
            return view;
        }
        
        private void OnRelease(TComponent resourceView)
        {
            resourceView.gameObject.SetActive(false);
            resourceView.transform.SetParent(_poolRoot,false);
        }
    }
}