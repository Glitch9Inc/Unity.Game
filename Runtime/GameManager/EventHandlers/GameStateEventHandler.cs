using System;

namespace Glitch9.Game
{
    public class GameStateEventHandler : IGameEventHandler
    {
        public EventHandler OnGameStart { get; set; }
        public EventHandler OnGameEnd { get; set; }
        public EventHandler OnGamePause { get; set; }
        public EventHandler OnGameResume { get; set; }
    }
}