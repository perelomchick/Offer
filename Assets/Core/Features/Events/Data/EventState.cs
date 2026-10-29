using System;

namespace Core.Features.Events.Data
{
    public readonly struct EventState 
    {
        public EventPhase Phase { get; }
        public TimeSpan Remaining { get; }

        public EventState(EventPhase phase, TimeSpan remaining)
        {
            Phase = phase;
            Remaining = remaining;
        }
    }
}