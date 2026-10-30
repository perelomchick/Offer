using System;
using Core.Features.Events.Data;
using Core.Features.TimeClock.Logic;
using Core.Features.TimeClock.Services;
using R3;
using UnityEngine;

namespace Core.Features.Events.View.Events
{
    public class EventPresenter : IDisposable
    {
        private readonly Subject<Unit> _eventChanged = new Subject<Unit>();
        private readonly EventView _eventView;
        private readonly CountdownTimer _countdownTimer;
        private readonly IDisposable _eventTimer;
        
        public Observable<Unit> EventChanged => _eventChanged;

        public EventPresenter(EventData  eventData,EventView eventView,EventWindowPresenter eventWindowPresenter, ITimeService timeService)
        {
            _eventView = eventView;
            
            _eventView.OpenWindow
                .Subscribe(_ => eventWindowPresenter.SetEventData(eventData))
                .AddTo(_eventView.gameObject);
            
            _countdownTimer = new CountdownTimer(eventData.TimeStartEvent, eventData.TimeEndEvent,timeService);
            
            _countdownTimer.OnStartTimer.Subscribe(_ => _eventChanged.OnNext(Unit.Default));
            _countdownTimer.OnEndTimer.Subscribe(_ => _eventChanged.OnNext(Unit.Default));
            
            _eventTimer = _countdownTimer.RemainingTime
                .Subscribe(time => _eventView.SetTimerText(time.ToString()))
                .AddTo(_eventView.gameObject);
        }

        public void Dispose()
        {
            if(_eventView != null)
                UnityEngine.Object.Destroy(_eventView.gameObject);
            
            _eventTimer?.Dispose();
        }
    }
}