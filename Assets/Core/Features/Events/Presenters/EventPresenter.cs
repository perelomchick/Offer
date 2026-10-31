using System;
using Core.Features.Events.Data;
using Core.Features.Events.View.Events;
using Core.Features.TimeClock.Logic;
using R3;

namespace Core.Features.Events.Presenters
{
    public class EventPresenter : IDisposable
    {
        private readonly Subject<Unit> _eventChanged = new();
        private readonly EventView _eventView;
        private readonly IDisposable _disposableTimer;
        
        public Observable<Unit> EventChanged => _eventChanged;

        public EventPresenter(EventData  eventData,EventView eventView,EventWindowPresenter eventWindowPresenter, CountdownTimer countdownTimer)
        {
            _eventView = eventView;
            
            _eventView.OpenWindow
                .Subscribe(_ => eventWindowPresenter.SetEventData(eventData))
                .AddTo(_eventView.gameObject);
            
            _disposableTimer = countdownTimer.RemainingTime
                .Subscribe(time => _eventView.SetTimerText(time.ToString()))
                .AddTo(_eventView.gameObject);
        }

        public void Dispose()
        {
            if(_eventView != null)
                UnityEngine.Object.Destroy(_eventView.gameObject);
            
            _disposableTimer?.Dispose();
        }
    }
}