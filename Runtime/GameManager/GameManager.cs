using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public IUser User { get; set; }
        public ILogger Logger { get; private set; }
        public List<IGameEventHandler> EventHandlers { get; set; } = new();

        protected override void Awake()
        {
            base.Awake();
            Logger = new GameLogger(GameSettings.ProjectName);
        }
    }
}