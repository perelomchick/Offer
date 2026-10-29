using System;
using Core.Common;
using Core.Events.Data;
using R3;

namespace Core.Events.View.Events
{
    public class EventPresenter : IDisposable
    {
        private readonly EventView _eventView;
        private readonly IDisposable _eventTimer;

        public EventPresenter(EventData  eventData,EventView eventView,EventWindowPresenter eventWindowPresenter)
        {
            _eventView = eventView;
            
            _eventView.OpenWindow
                .Subscribe(_ => eventWindowPresenter.SetEventData(eventData))
                .AddTo(_eventView.gameObject);
            
            _eventTimer = EventClock.Track(
                eventData.TimeStartEvent.ToDateTimeOffset(),
                eventData.TimeEndEvent.ToDateTimeOffset())
                .Subscribe(state => _eventView.SetTimerText(state.EventStateToString()))
                .AddTo(eventView.gameObject);
        }

        public void Dispose()
        {
            if(_eventView != null)
                UnityEngine.Object.Destroy(_eventView.gameObject);
            
            _eventTimer.Dispose();
        }
    }
}