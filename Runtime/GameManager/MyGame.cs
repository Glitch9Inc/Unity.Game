using Glitch9.Game.MailSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glitch9.Game
{
    public class MyGame
    {
        private static bool InstanceIsNull()
        {
            if (GameManager.Instance != null) return false;
            Debug.LogError("GameManager instance is null");
            return true;
        }

        private static TGameEventHandler GetGameEventHandler<TGameEventHandler>(bool create = false) where TGameEventHandler : class, IGameEventHandler
        {
            if (InstanceIsNull()) return null;
            TGameEventHandler handler = GameManager.Instance.EventHandlers.Find(e => e is TGameEventHandler) as TGameEventHandler;
            if (handler == null)
            {
                if (create)
                {
                    handler = Activator.CreateInstance<TGameEventHandler>();
                    GameManager.Instance.EventHandlers.Add(handler);
                }
                else
                {
                    Debug.LogError($"GameEventHandler {typeof(TGameEventHandler).Name} not found");
                }
            }
            return handler;
        }

        public static IUser User
        {
            get
            {
                if (InstanceIsNull()) return null;
                return GameManager.Instance.User;
            }
        }

        public static EventHandler OnGameStart
        {
            get => GetGameEventHandler<GameStateEventHandler>(true)?.OnGameStart;
            set
            {
                GameStateEventHandler handler = GetGameEventHandler<GameStateEventHandler>(true);
                if (handler != null) handler.OnGameStart = value;
            }
        }

        public static EventHandler OnGameEnd
        {
            get => GetGameEventHandler<GameStateEventHandler>(true)?.OnGameEnd;
            set
            {
                GameStateEventHandler handler = GetGameEventHandler<GameStateEventHandler>(true);
                if (handler != null) handler.OnGameEnd = value;
            }
        }

        public static EventHandler OnGamePause
        {
            get => GetGameEventHandler<GameStateEventHandler>(true)?.OnGamePause;
            set
            {
                GameStateEventHandler handler = GetGameEventHandler<GameStateEventHandler>(true);
                if (handler != null) handler.OnGamePause = value;
            }
        }

        public static EventHandler OnGameResume
        {
            get => GetGameEventHandler<GameStateEventHandler>(true)?.OnGameResume;
            set
            {
                GameStateEventHandler handler = GetGameEventHandler<GameStateEventHandler>(true);
                if (handler != null) handler.OnGameResume = value;
            }
        }

        public static EventHandler<Mail> OnMailReceived
        {
            get => GetGameEventHandler<MailEventHandler>(true)?.OnMailReceived;
            set
            {
                MailEventHandler handler = GetGameEventHandler<MailEventHandler>(true);
                if (handler != null) handler.OnMailReceived = value;
            }
        }

        public static void DisplayInfo(object sender, string message)
        {
            GetGameEventHandler<SystemEventHandler>()?.DisplayInfo(sender, message);
        }

        public static void DisplayWarning(object sender, string message)
        {
            GetGameEventHandler<SystemEventHandler>()?.DisplayWarning(sender, message);
        }

        public static void DisplayError(object sender, string message)
        {
            GetGameEventHandler<SystemEventHandler>()?.DisplayError(sender, message);
        }

        public static void DisplayIssue(object sender, Issue issue)
        {
            GetGameEventHandler<SystemEventHandler>()?.DisplayError(sender, issue.GetMessage());
        }

        public static void UpdateCurrency(object sender)
        {
            GetGameEventHandler<SystemEventHandler>()?.UpdateCurrency(sender, EventArgs.Empty);
        }

        public static void HandleResult(object sender, IResult result)
        {
            GetGameEventHandler<SystemEventHandler>()?.HandleResult(sender, result);
        }

        public static void GetRewards(object sender, IEnumerable<IReward> rewards)
        {
            GetGameEventHandler<SystemEventHandler>()?.GetRewards(sender, rewards);
        }

        public static void ExecuteTrade(object sender, ItemTrade trade)
        {
            GetGameEventHandler<SystemEventHandler>()?.ExecuteTrade(sender, trade);
        }

        public static void MakeAnnouncement(object sender, AnnounceType announceType, string message, bool isGlobal = false)
        {
            AnnouncementEventHandler handler = GetGameEventHandler<AnnouncementEventHandler>();
            if (handler == null) return;

            Announcement announcement = new Announcement
            {
                type = announceType,
                message = message,
                isGlobal = isGlobal
            };

            handler.MakeAnnouncement(sender, announcement);
        }
    }
}