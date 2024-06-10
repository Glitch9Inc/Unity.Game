using System;
using System.Collections.Generic;
using Glitch9.Game.MailSystem;

namespace Glitch9.Game
{
    public interface IGameEventHandler
    {
        // UI Events
        EventHandler<string> OnDisplayInfo { get; set; }
        EventHandler<string> OnDisplayWarning { get; set; }
        EventHandler<string> OnDisplayError { get; set; }

        // Data Change Events
        EventHandler OnCurrencyChanged { get; set; }
        EventHandler<IResult> OnResultReceived { get; set; }
        EventHandler<IEnumerable<IReward>> OnRewardReceived { get; set; }
        EventHandler<Mail> OnMailReceived { get; set; }
        EventHandler<int> OnExperienceGained { get; set; }
        EventHandler<int> OnSeasonPassExperienceGained { get; set; }

        EventHandler OnGameStart { get; set; }
        EventHandler<ItemTrade> OnItemTrade { get; set; }

        // Reset Events
        EventHandler<IResult> OnDailyReset { get; set; }
        EventHandler<IResult> OnWeeklyReset { get; set; }
    }
}