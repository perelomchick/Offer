using System.Collections.Generic;
using System.Linq;
using Core.Data;
using R3;
using UnityEngine;

namespace Core.Events.Services
{
    public class Inventory
    {//Условный класс Inventory, просто чтобы бы какой-то
        private readonly Dictionary<TypeResource, ReactiveProperty<int>> _resources = new()
        {
            { TypeResource.Gold,    new ReactiveProperty<int>(10) },
            { TypeResource.Wood,    new ReactiveProperty<int>(10) },
            { TypeResource.Stone,   new ReactiveProperty<int>(0) },
            { TypeResource.Diamond, new ReactiveProperty<int>(10) },
        };

        public Observable<int> GetObservableResource(TypeResource typeResource) =>
            _resources.TryGetValue(typeResource, out var prop) ? prop : Observable.Return(0);
        
        public int GetAmount(TypeResource res) =>
            _resources.TryGetValue(res, out var prop) ? prop.Value : 0;

        public void AddResource(TypeResource res, int count)
        {
            if (_resources.TryGetValue(res, out var prop)) prop.Value += count;
            else Debug.Log("Add resource Error: " + res);
        }

        public void RemoveResource(TypeResource res, int count)
        {
            if (_resources.TryGetValue(res, out var prop) && prop.Value >= count)
                prop.Value -= count;
            else
                Debug.Log("Remove resource Error: " + res);
        }

        public List<Resource> ToSaveData() =>
            _resources.Select(p => new Resource { Type = p.Key, Amount = p.Value.Value }).ToList();

        public void LoadFromSave(List<Resource> resources)
        {
            foreach (var res in resources)
                SetAmount(res.Type, res.Amount);
        }
        
        private void SetAmount(TypeResource res, int count)
        {
            if (_resources.TryGetValue(res, out var prop)) prop.Value = count;
        }
    }
}