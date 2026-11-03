using System;
using Core.Features.Events.Presenters;
using Core.Features.Events.UI.Events;

namespace Core.Features.Events.Data
{
    public class EventEntry : IDisposable
    {
        private readonly EventPresenter _eventPresenter;
        private readonly EventUI _eventUI;

        public EventEntry(EventPresenter eventPresenter, EventUI eventUI)
        {
            _eventPresenter = eventPresenter;
            _eventUI = eventUI;
        }
        
        public void Dispose()
        {
            _eventPresenter.Dispose();
            if(_eventUI != null)
                UnityEngine.Object.Destroy(_eventUI.gameObject);
        }
    }
}