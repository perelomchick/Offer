using System;

namespace Core.Data
{
    [Serializable]
    public class TimeDate 
    {
        public int Year = 2026;
        public int Month = 10;
        public int Day = 21;
        public int Hour = 12;
        public int Minute;
        public int Second;
        
        public int OffsetMinutes;

        public TimeDate() { }

        public TimeDate(DateTimeOffset value)
        {
            Year = value.Year;
            Month = value.Month;
            Day = value.Day;
            Hour = value.Hour;
            Minute = value.Minute;
            Second = value.Second;
            OffsetMinutes = (int)value.Offset.TotalMinutes;
        }

        public DateTimeOffset ToDateTimeOffset()
        {
            int month = Math.Clamp(Month, 1, 12);
            int year = Math.Clamp(Year, 1, 9999);
            int day = Math.Clamp(Day, 1, DateTime.DaysInMonth(year, month));
            int hour = Math.Clamp(Hour, 0, 23);
            int minute = Math.Clamp(Minute, 0, 59);
            int second = Math.Clamp(Second, 0, 59);
            int offset = Math.Clamp(OffsetMinutes, -840, 840);

            return new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.FromMinutes(offset));
        }
    }
}