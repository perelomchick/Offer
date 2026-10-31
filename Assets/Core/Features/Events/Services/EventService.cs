using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Core.Features.Events.Data;
using Core.Features.Events.Factories;
using Core.Features.TimeClock.Services;

namespace Core.Features.Events.Services
{
    public class EventService : IDisposable
    {
        private readonly Dictionary<int, EventEntry> _events = new();
        private readonly EventEntryFactory _eventEntryFactory;
        private readonly PurchaseService _purchaseService;
        private readonly ITimeService _timeService;

        private IDisposable _disposableFactoryTimer;
        private EventData[] _eventData;

        public EventService(EventEntryFactory eventEntryFactory, PurchaseService purchaseService,ITimeService timeService)
        {
            _timeService = timeService;
            _eventEntryFactory = eventEntryFactory;
            _purchaseService = purchaseService;
            _disposableFactoryTimer = _eventEntryFactory.TimeChanged.Subscribe(_ => RefreshEvents());
        }

        public void StartEvent(EventData[] eventsData)
        {
            _eventData = (EventData[])eventsData.Clone();
            _purchaseService.OfferPurchased.Subscribe(_ => RefreshEvents());

            RefreshEvents();
        }

        private void RefreshEvents()
        {
            var now = _timeService.GetNow();

            foreach (var eventData in _eventData)
            {
                bool isActive = eventData.TimeStartEvent <= now
                                && eventData.TimeEndEvent > now;

                var isCompleted = AllOffersPurchased(eventData);
                bool isContains = _events.ContainsKey(eventData.ID);

                if (isActive && !isCompleted && !isContains)
                {
                    _events.Add(eventData.ID, _eventEntryFactory.CreateEventView(eventData));
                }
                else if ((!isActive || isCompleted) && isContains)
                {
                    _events[eventData.ID].Dispose();
                    _events.Remove(eventData.ID);
                }
            }
        }

        private bool AllOffersPurchased(EventData eventData)
        {
            return eventData.Offers.All(offer => _purchaseService.IsPurchased(eventData, offer));
        }

        public void Dispose()
        {
            foreach (var eventEntry in _events.Values)
            {
                eventEntry.Dispose();
            }
            
            _disposableFactoryTimer.Dispose();
            _events.Clear();
        }
    }
}