using System.Collections.Generic;
using Glitch9.DB;

namespace Glitch9.Game
{
    public class ItemPoolRegistry
    {
        private static readonly Dictionary<int, IItemPool> _pools = new();

        public static void Register(int poolId, IItemPool pool)
        {
            _pools[poolId] = pool;
        }

        public static IItemPool Get(int poolId)
        {
            return _pools.GetValueOrDefault(poolId);
        }
    }
}