using Core.Features.Events.Data;

namespace Core.Features.TimeClock.Logic
{
    public static class EventStateExtensionToString
    {//Отерепить зависимость от EventState
        public static  string EventStateToString(this EventState state)
        {
            var t = state.Remaining;
            var time = t.TotalHours >= 1
                ? $"{(int)t.TotalHours:00}:{t.Minutes:00}:{t.Seconds:00}"
                : $"{t.Minutes:00}:{t.Seconds:00}";

            return state.Phase switch
            {
                EventPhase.Upcoming => $"До начала: {time}",
                EventPhase.Running => $"{time}",
                _ => "Ивент завершён",
            };
        }
    }
}