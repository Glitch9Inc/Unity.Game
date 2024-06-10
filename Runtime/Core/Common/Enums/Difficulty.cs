using Glitch9.Apis.Google.Sheets;

namespace Glitch9.Game
{
    [Sheets(typeof(Difficulty))]
    public enum Difficulty : short
    {
        None = 0,
        Easy,
        Medium,
        Hard,
        VeryHard,
        Varies = 99,
    }
}