using Glitch9.Game.MailSystem;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public interface IUser
    {
        IDictionary<string, Mail> Mails { get; set; }
        IList<int> ReceivedSystemMails { get; set; }
    }
}