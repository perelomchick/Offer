using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Presenters;
using Core.Features.Events.UI.Events;
using Core.Features.TimeClock.Logic;
using Core.Features.TimeClock.Services;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Logic;
using Core.Infrastructure.AssetManager.Services;
using R3;
using UnityEngine;

namespace Core.Features.Events.Factories
{
    public class EventEntryFactory
    {
        private readonly ITimeService _timeService;
        private readonly Transform _parent;
        private readonly EventWindowPresenter _windowPresenter;
        private readonly EventUI _uiPrefab;
        private readonly Subject<Unit> _timeChanged = new Subject<Unit>();
        
        public Observable<Unit> TimeChanged => _timeChanged;

        public EventEntryFactory(AddressableContainer addressableContainer,
            ITimeService timeService,
            EventContainer eventContainer,
            EventWindowPresenter windowPresenter)
        {
            _windowPresenter = windowPresenter;
            _timeService = timeService;
            _parent = eventContainer.GetContainer();
            
            _uiPrefab = addressableContainer.GetViewPrefab<EventUI>(UiElements.EventView);
        }

        public EventEntry CreateEventView(EventData eventData)
        {
            var eventView = Object.Instantiate(_uiPrefab, _parent);
            var countdownTimer = CreateTimer(eventData);

            countdownTimer.OnStartTimer.Subscribe(_ => _timeChanged.OnNext(Unit.Default));
            countdownTimer.OnEndTimer.Subscribe(_ => _timeChanged.OnNext(Unit.Default));
            
            var eventPresenter = new EventPresenter(eventData, eventView, _windowPresenter,countdownTimer);
            return new EventEntry(eventPresenter, eventView);
        }

        private CountdownTimer CreateTimer(EventData eventData)
        {
            var countdownTimer = new CountdownTimer(eventData.TimeStartEvent, eventData.TimeEndEvent,_timeService);

            Observable
                .Merge(countdownTimer.OnEndTimer, countdownTimer.OnStartTimer)
                .Subscribe(_=>_timeChanged.OnNext(Unit.Default));
            return countdownTimer;
        }
    }
}