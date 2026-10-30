using System;
using Core.Features.TimeClock.Services;
using R3;

namespace Core.Features.TimeClock.Logic
{
    public class CountdownTimer : IDisposable
    {
        private readonly ReactiveProperty<TimeSpan> _remainingTime = new();
        private readonly Subject<Unit> _onStartTimer = new();
        private readonly Subject<Unit> _onEndTimer = new();

        private readonly DateTimeOffset _dateStart;
        private readonly DateTimeOffset _dateEnd;
        private readonly ITimeService _timeService;
        
        private IDisposable _tick;
        
        public Observable<Unit> OnStartTimer => _onStartTimer;
        public Observable<Unit> OnEndTimer => _onEndTimer;
        public Observable<TimeSpan> RemainingTime => _remainingTime;

        public CountdownTimer(DateTimeOffset dateStart,
            DateTimeOffset dateEnd,
            ITimeService timeService)
        {
            _dateStart = dateStart;
            _dateEnd = dateEnd;
            _timeService = timeService;
            
            Start();
        }

        private void Start()
        {
            var now = _timeService.GetNow();
            var duration =  _dateStart - now;

            if (_dateEnd <= now)
            {
                EndCountdown();
                return;
            }

            if (duration > TimeSpan.Zero)
            {
                _tick = Observable.Timer(duration,UnityTimeProvider.Update).Subscribe(_ => BeginCountdown());
                return;
            }
            
            BeginCountdown();
        }

        private void BeginCountdown()
        {
            _tick?.Dispose();
            _onStartTimer.OnNext(Unit.Default);
            _tick = Observable.Interval(TimeSpan.FromSeconds(1f),UnityTimeProvider.Update)
                .Subscribe(_ => Tick());
            Tick();
        }

        private void Tick()
        {
            var now = _timeService.GetNow();
            
            if (_dateEnd <= now)
            {
                _remainingTime.Value = TimeSpan.Zero;
                EndCountdown();
                return;
            }
            
            var remainingToSecond = Math.Ceiling((_dateEnd - now).TotalSeconds);
            _remainingTime.Value = TimeSpan.FromSeconds(remainingToSecond);
        }
        
        private void EndCountdown()
        {
            _tick?.Dispose();

            _onEndTimer.OnNext(Unit.Default);
            _onEndTimer.OnCompleted();
            _onStartTimer.OnCompleted();
        }
        
        public void Dispose()
        {
            _tick?.Dispose();
            _remainingTime?.Dispose();
            _onStartTimer?.Dispose();
            _onEndTimer?.Dispose();
        }
    }
}

