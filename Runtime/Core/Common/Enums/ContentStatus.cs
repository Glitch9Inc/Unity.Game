
using Glitch9.Apis.Google.Sheets;

namespace Glitch9.Game
{
    [Sheets(typeof(ContentStatus))]
    public enum ContentStatus
    {
        Unknown,
        Locked,
        Unlocked,
    }
}
