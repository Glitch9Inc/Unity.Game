using System;

namespace Glitch9
{

    [Serializable]
    public class TimeRange
    {
        public virtual int StartHour { get; set; }
        public virtual int StartMinute { get; set; }
        public virtual int EndHour { get; set; }
        public virtual int EndMinute { get; set; }
        public virtual int Duration
        {
            get => (int)(EndTime - StartTime).TotalMinutes;
            set => EndTime = StartTime.AddMinutes(value);
        }


        public DateTime StartTime
        {
            get => DateTimeExtensions.Today(StartHour, StartMinute);
            set
            {
                StartHour = value.Hour;
                StartMinute = value.Minute;
            }
        }

        public DateTime EndTime
        {
            get => DateTimeExtensions.Today(EndHour, EndMinute, StartTime);
            set
            {
                EndHour = value.Hour;
                EndMinute = value.Minute;
            }
        }

        public ClockTime StartHrMin => new ClockTime(StartHour, StartMinute);
        public ClockTime EndHrMin => new ClockTime(EndHour, EndMinute);

        public override string ToString()
        {
            //format : 00:00-00:00
            return $"[{StartHour:D2}:{StartMinute:D2}-{EndHour:D2}:{EndMinute:D2}]";
        }

        public TimeRange(string timeRangeAsString)
        {
            //format : 00:00-00:00
            string[] times = timeRangeAsString.Split('-');
            string[] start = times[0].Split(':');
            string[] end = times[1].Split(':');

            StartHour = int.Parse(start[0]);
            StartMinute = int.Parse(start[1]);
            EndHour = int.Parse(end[0]);
            EndMinute = int.Parse(end[1]);
        }

        public TimeRange() { }

        public TimeRange(int startHour, int startMinute, int endHour, int endMinute)
        {
            StartHour = startHour;
            StartMinute = startMinute;
            EndHour = endHour;
            EndMinute = endMinute;
        }

        public static TimeRange Parse(string timeRangeAsString) => new TimeRange(timeRangeAsString);

        public bool IsOverlapping(TimeRange timeRange)
        {
            if (StartHour > timeRange.EndHour || timeRange.StartHour > EndHour) return false;
            if (StartHour == timeRange.EndHour && StartMinute > timeRange.EndMinute) return false;
            if (timeRange.StartHour == EndHour && timeRange.StartMinute > EndMinute) return false;
            return true;
        }

        public bool IsWithinRange(int hour, int minute)
        {
            DateTime time = new DateTime(1, 1, 1, hour, minute, 0);
            return IsWithinRange(time);
        }

        public bool IsWithinRange(DateTime time)
        {
            return time >= StartTime && time <= EndTime;
        }

        public bool IsAllDay => StartHour == 1 && StartMinute == 0 && EndHour == 24 && EndMinute == 0;
        public bool IsNoTime => StartHour == 0 && StartMinute == 0 && EndHour == 0 && EndMinute == 0;

        public bool Contains(DateTime dateTime)
        {
            if (IsAllDay) return true;
            if (IsNoTime) return false;
            return dateTime >= StartTime && dateTime < EndTime;
        }

    }
}
