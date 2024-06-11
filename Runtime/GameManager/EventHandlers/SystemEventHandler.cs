using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
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
}