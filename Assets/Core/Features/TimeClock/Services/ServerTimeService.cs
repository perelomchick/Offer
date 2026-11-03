using System;

namespace Core.Features.TimeClock.Services
{
    public class ServerTimeService : ITimeService
    {
        public DateTimeOffset GetNow()
        {
            return DateTimeOffset.Now;
        }
    }
}