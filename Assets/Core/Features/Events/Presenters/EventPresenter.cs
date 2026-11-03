using R3;
using System;
using Core.Features.Events.Data;
using Core.Features.Events.UI.Events;
using Core.Features.TimeClock.Logic;

namespace Core.Features.Events.Presenters
{
    public class EventPresenter : IDisposable
    {
        private readonly Subject<Unit> _eventChanged = new();
        private readonly EventUI _eventUI;
        private readonly IDisposable _disposableTimer;
        
        public Observable<Unit> EventChanged => _eventChanged;

        public EventPresenter(EventData  eventData,EventUI eventUI,EventWindowPresenter eventWindowPresenter, CountdownTimer countdownTimer)
        {
            _eventUI = eventUI;
            
            _eventUI.OpenWindow
                .Subscribe(_ => eventWindowPresenter.SetEventData(eventData))
                .AddTo(_eventUI.gameObject);
            
            _disposableTimer = countdownTimer.RemainingTime
                .Subscribe(time => _eventUI.SetTimerText(time.ToString()))
                .AddTo(_eventUI.gameObject);
        }

        public void Dispose()
        {
            if(_eventUI != null)
                UnityEngine.Object.Destroy(_eventUI.gameObject);
            
            _disposableTimer?.Dispose();
        }
    }
}