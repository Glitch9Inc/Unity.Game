using System;

namespace Glitch9.Game
{
    public class ProgressEventHandler : IGameEventHandler
    {
        public EventHandler<int> OnExperienceGained { get; set; }
        public EventHandler<int> OnLevelUp { get; set; }
        public EventHandler<int> OnSeasonPassExperienceGained { get; set; }
        public EventHandler<int> OnSeasonPassLevelUp { get; set; }
    }
}