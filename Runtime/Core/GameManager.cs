using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public IUser User { get; set; }
        public IGameEventHandler EventHandler { get; private set; }
        public ILogger Logger { get; private set; }

        public void Initialize(IGameEventHandler eventHandler, ILogger customLogger = null)
        {
            EventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
            Logger = customLogger ?? new GameLogger(GameSettings.ProjectName);
        }

        public void DisplayInfo(object sender, string message)
        {
            EventHandler.OnDisplayInfo?.Invoke(sender, message);
        }

        public void DisplayWarning(object sender, string message)
        {
            EventHandler.OnDisplayWarning?.Invoke(sender, message);
        }

        public void DisplayError(object sender, string message)
        {
            EventHandler.OnDisplayError?.Invoke(sender, message);
        }

        public void CurrencyChanged(object sender)
        {
            EventHandler.OnCurrencyChanged?.Invoke(sender, EventArgs.Empty);
        }

        public void ResultReceived(object sender, IResult result)
        {
            EventHandler.OnResultReceived?.Invoke(sender, result);
        }

        public void RewardsReceived(object sender, IEnumerable<IReward> rewards)
        {
            EventHandler.OnRewardReceived?.Invoke(sender, rewards);
        }

        public void DisplayIssue(object sender, Issue issue)
        {
            EventHandler.OnDisplayError?.Invoke(sender, issue.GetMessage());
        }
    }
}