using System;

namespace Core.Features.TimeClock.Services
{
    public interface ITimeService
    {
        public DateTimeOffset GetNow();
    }
}