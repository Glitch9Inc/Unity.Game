using System;

namespace Glitch9.Game
{
    /// <summary>
    /// StartDailyTimer()를 호출할 경우 오늘 하루가 끝나는 시간을 기준으로 남은 시간을 계산합니다.
    /// StartWeeklyTimer()를 호출할 경우 이번주가 끝나는 시간을 기준으로 남은 시간을 계산합니다.
    /// </summary>
    public class ResetTimer
    {
        public TimeSpan RemainingTime => targetTime - DateTime.Now;
        public bool IsPlaying => RemainingTime > TimeSpan.Zero;

        private DateTime targetTime;
        private DateTime timeStarted;
        private DateTime timePaused;


        public void StartDailyTimer()
        {
            var resetDay = DateTime.Now.DayOfWeek;
            ClockTime resetTime = GameSettings.DailyResetTime;

            targetTime = GetNextResetTime(resetDay, resetTime);
            StartTimer();
        }

        public void StartWeeklyTimer()
        {
            DayOfWeek resetDay = GameSettings.WeeklyResetDay;
            ClockTime resetTime = GameSettings.DailyResetTime;

            targetTime = GetNextResetTime(resetDay, resetTime);
            StartTimer();
        }

        private DateTime GetNextResetTime(DayOfWeek resetDay, ClockTime resetTime)
        {
            var now = DateTime.Now;
            var nextResetTime = new DateTime(now.Year, now.Month, now.Day, resetTime.Hour, resetTime.Minute, 0);

            if (resetDay == nextResetTime.DayOfWeek)
            {
                if (now.Hour >= resetTime.Hour)
                {
                    nextResetTime = nextResetTime.AddDays(1);
                }
            }
            else
            {
                while (nextResetTime.DayOfWeek != resetDay)
                {
                    nextResetTime = nextResetTime.AddDays(1);
                }
            }

            return nextResetTime;
        }

        public void Set(ClockTime time, int untilDay = 0)
        {
            targetTime = DateTime.Now.AddHours(time.Hour).AddMinutes(time.Minute).AddDays(untilDay);
            StartTimer();
        }

        private void StartTimer()
        {
            timeStarted = DateTime.Now;
        }

        public void PauseTimer()
        {
            timePaused = DateTime.Now;
        }

        public void ResumeTimer()
        {
            timeStarted = DateTime.Now - (timePaused - timeStarted);
        }

        public void Reset()
        {
            targetTime = DateTime.MinValue;
            timeStarted = DateTime.MinValue;
            timePaused = DateTime.MinValue;
        }
    }
}