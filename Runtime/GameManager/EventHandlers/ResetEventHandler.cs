using System;

namespace Glitch9.Game
{
    public class ResetEventHandler : IGameEventHandler
    {
        public EventHandler<IResult> OnDailyReset { get; set; }
        public EventHandler<IResult> OnWeeklyReset { get; set; }
    }
}