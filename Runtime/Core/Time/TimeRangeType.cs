using UnityEngine;

namespace Glitch9.Apis.Google.Sheets
{
    [Type(typeof(TimeRange), new string[] { "TimeRange" })]
    public class TimeRangeType : IType
    {
        public object DefaultValue => new TimeRange();
        /// <summary>
        /// value는 스프레드 시트에 적혀있는 값
        /// </summary> 
        public object Read(string value)
        {
            // 0 = 0 hours (No time)
            // 1 = 24 hours (All day)
            // [16:00-23:00] = 16:00-23:00
            // [18:00-24:00] = 18:00-24:00
            // 무조건 01부터 시작한다. 00은 없다.

            if (string.IsNullOrEmpty(value)) return DefaultValue;
            if (value.Contains("["))
            {
                if (!value.Contains("]"))
                {
                    Debug.LogError($"TimeRangeType.Read() : Invalid time range format. {value}");
                    return DefaultValue;
                }

                string[] times = value.Replace("[", "").Replace("]", "").Split('-');
                string[] start = times[0].Split(':');
                string[] end = times[1].Split(':');

                if (start.Length != 2 || end.Length != 2)
                {
                    Debug.LogError($"TimeRangeType.Read() : Invalid time range format. {value}");
                    return DefaultValue;
                }

                int.TryParse(start[0], out int startHour);
                int.TryParse(start[1], out int startMinute);

                int.TryParse(end[0], out int endHour);
                int.TryParse(end[1], out int endMinute);

                return new TimeRange(startHour, startMinute, endHour, endMinute);
            }

            int.TryParse(value, out int valueAsInt);
            if (valueAsInt != 0 && valueAsInt != 1)
            {
                Debug.LogError($"TimeRangeType.Read() : Invalid time range format. {value}");
                return DefaultValue;
            }
            else if (valueAsInt == 0)
            {
                return new TimeRange();
            }
            else
            {
                return new TimeRange(1, 24, 0, 0);
            }
        }

        public string Write(object value)
        {
            TimeRange timeRange = (TimeRange)value;
            if (timeRange.IsAllDay) return "1";
            else if (timeRange.IsNoTime) return "0";
            else return timeRange.ToString();
        }
    }
}