using System;
using Glitch9.Game.MailSystem;

namespace Glitch9.Game
{
    public class MailEventHandler : IGameEventHandler
    {
        public EventHandler<Mail> OnMailReceived { get; set; }
    }
}