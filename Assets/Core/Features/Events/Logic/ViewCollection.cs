using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Features.Events.Logic
{
    public class ViewCollection<T> where T : MonoBehaviour
    {
        private readonly ViewPool<T> _pool;
        private readonly List<T> _active = new();
        
        public ViewCollection(T prefab, Transform parent)
        {
            _pool = new ViewPool<T>(prefab, parent);
        }

        
        //Так и не понял почему это работает
        //Подробнее узнать про ObjectPool и LIFO
        public List<T> Fill<TData>(IEnumerable<TData> data, Action<T, TData> bind)
        {
            ReleaseAll();

            int index = 0;
            foreach (var item in data)
            {
                var view = _pool.Get();
                view.transform.SetSiblingIndex(index);
                bind(view, item);
                _active.Add(view);
                index++;
            }

            return _active;
        }

        public void ReleaseAll()
        {
            // Обратный порядок: последний возвращённый в стек достаётся первым,
            // поэтому так первый элемент снова окажется первым
            for (int i = _active.Count - 1; i >= 0; i--)
                _pool.Release(_active[i]);

            _active.Clear();
        }
        
        public void Dispose()
        {
            _active.Clear();
            _pool.Clear();
        }
    }
}