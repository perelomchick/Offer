using System;
using System.Collections.Generic;
using System.Linq;
using OfferCore.Common;
using OfferCore.Data;
using OfferCore.Events.Data;
using OfferCore.Events.View;
using OfferCore.Events.View.Events;
using R3;
using UnityEngine;
using Zenject;
using Object = System.Object;

namespace OfferCore.Events.Services
{
    public class EventService : IDisposable
    {
        private readonly Dictionary<int, (EventPresenter presenter, EventView view)> _events = new();

        private AddressableContainer _addressableContainer;
        private PurchaseService _purchaseService;
        private EventView _eventViewPrefab;
        private EventData[] _eventData;
        private Transform _parent;
        private EventWindowUI _eventWindowUI;
        private EventWindowPresenter _eventWindowPresenter;

        [Inject]
        public void Construct(AddressableContainer addressableContainer, PurchaseService purchaseService)
        {
            _addressableContainer = addressableContainer;
            _purchaseService = purchaseService;
        }

        public void StartEvent(EventData[] eventsData, Transform parent)
        {
            _eventData = (EventData[])eventsData.Clone();
            _parent = parent;

            var prefab = _addressableContainer.GetViewPrefab<EventWindowUI>(ViewEnum.EventWindowUI);
            _eventWindowUI = UnityEngine.Object.Instantiate(prefab);
            _eventWindowPresenter = new EventWindowPresenter(_eventWindowUI, _addressableContainer, _purchaseService);

            _eventViewPrefab = _addressableContainer.GetViewPrefab<EventView>(ViewEnum.EventView);

            _purchaseService.EventCompleted += OnEventCompleted;

            RefreshEvents();
        }

        public void RefreshEvents()
        {
            var now = DateTimeOffset.Now;

            foreach (var eventData in _eventData)
            {
                bool isActive = eventData.TimeStartEvent.ToDateTimeOffset() <= now
                                && eventData.TimeEndEvent.ToDateTimeOffset() > now;
                bool isCompleted = _purchaseService.IsEventCompleted(eventData);
                bool isShown = _events.ContainsKey(eventData.ID);

                if (isActive && !isCompleted && !isShown)
                    CreateEventPresenter(eventData);
                else if ((!isActive || isCompleted) && isShown)
                    RemoveEventPresenter(eventData.ID);
            }
        }

        private void CreateEventPresenter(EventData eventData)
        {
            var eventView = UnityEngine.Object.Instantiate(_eventViewPrefab, _parent);
            var eventPresenter = new EventPresenter(eventData, eventView, _eventWindowPresenter);
            _events.Add(eventData.ID, (eventPresenter, eventView));
        }

        private void RemoveEventPresenter(int eventId)
        {
            var (presenter, view) = _events[eventId];
            presenter.Dispose();

            if (view != null)
                UnityEngine.Object.Destroy(view.gameObject);

            _events.Remove(eventId);
        }

        private void OnEventCompleted(EventData eventData)
        {
            // Купить оффер можно только из открытого окна, поэтому закрываем его
            _eventWindowUI.Hide();

            if (_events.ContainsKey(eventData.ID))
                RemoveEventPresenter(eventData.ID);
        }

        public void Dispose()
        {
            if (_purchaseService != null)
                _purchaseService.EventCompleted -= OnEventCompleted;

            foreach (var entry in _events.Values)
            {
                entry.presenter.Dispose();
                if (entry.view != null)
                    UnityEngine.Object.Destroy(entry.view.gameObject);
            }

            _events.Clear();

            _eventWindowPresenter?.Dispose();
            if (_eventWindowUI != null)
                UnityEngine.Object.Destroy(_eventWindowUI.gameObject);
        }
    }
}