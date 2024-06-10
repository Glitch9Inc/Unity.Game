using Glitch9.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glitch9
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = UnityMenu.Game.CREATE_GAME_SETTINGS, order = UnityMenu.Game.ORDER_CREATE_GAME_SETTINGS)]
    public class GameSettings : ScriptableResource<GameSettings>
    {
        private static class DefaultValues
        {
            internal const string PROJECT_NAME = "My Game";
            internal const int FRAME_RATE = 30;
            internal const int DAILY_RESET_TIME_HOUR = 3;
            internal const int DAILY_RESET_TIME_MINUTE = 0;
            internal const DayOfWeek WEEKLY_RESET_DAY = DayOfWeek.Wednesday;
            internal const bool IGNORE_VERSION_CHECK = false;
            internal const bool SKIP_TUTORIALS = false;
        }

        [Header("General Settings")]
        [SerializeField] private string projectName = DefaultValues.PROJECT_NAME;

        [Header("Graphics Settings")]
        [SerializeField] private int defaultFrameRate = DefaultValues.FRAME_RATE;

        [Header("Reset Settings")]
        [SerializeField] private ClockTime dailyResetTime = new(DefaultValues.DAILY_RESET_TIME_HOUR, DefaultValues.DAILY_RESET_TIME_MINUTE);
        [SerializeField] private DayOfWeek weeklyResetDay = DefaultValues.WEEKLY_RESET_DAY;

        [Header("Admins Accounts")]
        [SerializeField] private List<string> adminAccounts = new();

        [Header("Debug Settings")]
        [SerializeField] private bool ignoreVersionCheck = DefaultValues.IGNORE_VERSION_CHECK;
        [SerializeField] private bool skipTutorials = DefaultValues.SKIP_TUTORIALS;


        public static string ProjectName => Instance.projectName;
        public static int DefaultFrameRate => Instance.defaultFrameRate;
        public static ClockTime DailyResetTime => Instance.dailyResetTime;
        public static DayOfWeek WeeklyResetDay => Instance.weeklyResetDay;
        public static List<string> AdminAccounts => Instance.adminAccounts;
        public static bool IsAdmin(string email) => AdminAccounts.Contains(email);
        public static bool IgnoreVersionCheck => Instance.ignoreVersionCheck;
        public static bool SkipTutorials => Instance.skipTutorials;
    }
}