using System;
using Core.Features.Events.Presenters;
using Core.Features.Events.View.Events;

namespace Core.Features.Events.Data
{
    public class EventEntry : IDisposable
    {
        private readonly EventPresenter _eventPresenter;
        private readonly EventView _eventView;

        public EventEntry(EventPresenter eventPresenter, EventView eventView)
        {
            _eventPresenter = eventPresenter;
            _eventView = eventView;
        }
        
        public void Dispose()
        {
            _eventPresenter.Dispose();
            if(_eventView != null)
                UnityEngine.Object.Destroy(_eventView.gameObject);
        }
    }
}