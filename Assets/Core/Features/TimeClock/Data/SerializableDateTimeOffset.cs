using System;
using System.Globalization;
using UnityEngine;

namespace Core.Features.TimeClock.Data
{
    [Serializable]
    public class SerializableDateTimeOffset
    {
        public const string Format = "yyyy-MM-dd HH:mm:ss zzz";

        [SerializeField] private string _value;

        public SerializableDateTimeOffset(DateTimeOffset value)
        {
            _value = value.ToString(Format, CultureInfo.InvariantCulture);
        }

        public bool TryGet(out DateTimeOffset result)
            => DateTimeOffset.TryParse(
                _value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,   // если смещение не указано, считается UTC
                out result);

        public DateTimeOffset Value
        {
            get
            {
                if (TryGet(out var result))
                    return result;

                throw new FormatException($"Invalid date '{_value}'. Expected: {Format}");
            }
        }

        public static implicit operator DateTimeOffset(SerializableDateTimeOffset v) => v.Value;
        public static implicit operator SerializableDateTimeOffset(DateTimeOffset v) => new(v);
    }
}