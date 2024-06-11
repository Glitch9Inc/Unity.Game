using Glitch9.Game.MailSystem;
using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public interface IGameEventHandler
    {
    }


    public class ProgressEventHandler : IGameEventHandler
    {
        public EventHandler<int> OnExperienceGained { get; set; }
        public EventHandler<int> OnLevelUp { get; set; }
        public EventHandler<int> OnSeasonPassExperienceGained { get; set; }
        public EventHandler<int> OnSeasonPassLevelUp { get; set; }
    }

    public class MailEventHandler : IGameEventHandler
    {
        public EventHandler<Mail> OnMailReceived { get; set; }
    }

    public class GameStateEventHandler : IGameEventHandler
    {
        public EventHandler OnGameStart { get; set; }
        public EventHandler OnGameEnd { get; set; }
        public EventHandler OnGamePause { get; set; }
        public EventHandler OnGameResume { get; set; }
    }

    public class SystemEventHandler : IGameEventHandler
    {
        public EventHandler UpdateCurrency { get; set; }
        public EventHandler<ItemTrade> ExecuteTrade { get; set; }
        public EventHandler<IResult> HandleResult { get; set; }
        public EventHandler<IEnumerable<IReward>> GetRewards { get; set; }
        public EventHandler<string> DisplayInfo { get; set; }
        public EventHandler<string> DisplayWarning { get; set; }
        public EventHandler<string> DisplayError { get; set; }
    }

    public class AnnouncementEventHandler : IGameEventHandler
    {
        public EventHandler<Announcement> MakeAnnouncement { get; set; }
    }

    public class ResetEventHandler : IGameEventHandler
    {
        public EventHandler<IResult> OnDailyReset { get; set; }
        public EventHandler<IResult> OnWeeklyReset { get; set; }
    }
}