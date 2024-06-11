using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public static class GameResetUtils
    {
        /// <summary>
        /// Executes the daily reset of game data.
        /// </summary>
        public static void CheckDailyReset()
        {
            //UnixTime gameStart = User.Time.SessionStart;
            //if (gameStart.Date != UnixTime.Today.Date)
            //{
            //    User.Time.SessionStart = DateTime.Now;
            //}

            //UnixTime lastReset = User.Time.LastDailyReset;
            //if (lastReset.Date == UnixTime.Today.Date) return;

            //GNLog.Info("Daily Reset is executed.");
            //User.Time.LastDailyReset = DateTime.Today;
            //ResetPedometer();
            //MissionManager.Instance.ResetMissions(ResetPeriod.Daily);
            //GameManager.Instance.OnDayChanged?.Invoke();
        }

        private static DateTime CalculateLastDailyResetDate()
        {
            return new(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 6, 0, 0);
        }

        /// <summary>
        /// Executes the weekly reset of game data.
        /// </summary>
        public static bool CheckWeeklyReset(DateTime lastWeeklyReset)
        {
            DateTime lastReset = CalculateLastWeeklyResetDate();
            if (lastWeeklyReset > lastReset) return false;
            return true;
        }

        private static DateTime CalculateLastWeeklyResetDate()
        {
            DayOfWeek resetDay = GameSettings.WeeklyResetDay;
            int daysFromLastResetDay = ((int)DateTime.Today.DayOfWeek - (int)resetDay + 7) % 7;
            DateTime lastResetDay = DateTime.Today.AddDays(-daysFromLastResetDay);
            return new(lastResetDay.Year, lastResetDay.Month, lastResetDay.Day, 6, 0, 0);
        }
      
    }
}
