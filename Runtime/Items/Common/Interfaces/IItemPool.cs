using Glitch9.Game;

namespace Glitch9.DB
{
    public interface IItemPool
    {
        string[] GetPoolIds();
        ItemReward[] DrawItems();
    }
}