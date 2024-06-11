using System;

namespace Glitch9.Game
{
    public class AnnouncementEventHandler : IGameEventHandler
    {
        public EventHandler<Announcement> MakeAnnouncement { get; set; }
    }
}