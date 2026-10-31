using Core.Features.Events.Data;
using Core.Features.Events.Logic;
using Core.Features.Events.Presenters;
using Core.Features.Events.View.Events;
using Core.Features.TimeClock.Logic;
using Core.Features.TimeClock.Services;
using Core.Infrastructure.AssetManager.Data;
using Core.Infrastructure.AssetManager.Services;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Features.Events.Factories
{
    public class EventEntryFactory
    {
        private readonly ITimeService _timeService;
        private readonly Transform _parent;
        private readonly EventWindowPresenter _windowPresenter;
        private readonly EventView _viewPrefab;
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
            
            _viewPrefab = addressableContainer.GetViewPrefab<EventView>(UiElements.EventView);
        }

        public EventEntry CreateEventView(EventData eventData)
        {
            var eventView = Object.Instantiate(_viewPrefab, _parent);
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