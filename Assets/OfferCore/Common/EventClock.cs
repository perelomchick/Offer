using System;
using OfferCore.Events.Data;
using R3;

namespace OfferCore.Common
{
    public static class EventClock
    {
        public static Observable<EventState> Track(
            DateTimeOffset start,
            DateTimeOffset end,
            Func<DateTimeOffset> currentTime = null)
        {
            if (end <= start) throw new ArgumentException("Конец должен быть позже начала");

            currentTime ??= () => DateTimeOffset.UtcNow;
            var finished = new EventState(EventPhase.Finished, TimeSpan.Zero);

            return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Select(_ => Compute(currentTime(), start, end))
                .TakeWhile(s => s.Phase != EventPhase.Finished)
                .Concat(Observable.Return(finished));
        }

        private static EventState Compute(DateTimeOffset now, DateTimeOffset start, DateTimeOffset end)
        {
            if (now < start)
                return new EventState(EventPhase.Upcoming, start - now);
            
            if (now < end)
                return new EventState(EventPhase.Running, end - now);
            
            return new EventState(EventPhase.Finished, TimeSpan.Zero);
        }
        
     
    }
}

