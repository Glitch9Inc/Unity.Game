namespace Glitch9.Game.HardCodedTables
{
    public class CharacterStatExpTable : IExpTable
    {
        // 최대 50레벨
        readonly int[] Table =
        {
            100, 115, 132, 152, 175, 201, 231, 266, 306, 351,
            404, 464, 533, 613, 704, 809, 930, 1069, 1229, 1413,
            1625, 1868, 2146, 2462, 2820, 3225, 3681, 4195, 4772, 5420,
            6146, 6959, 7870, 8888, 10023, 11285, 12787, 14542, 16565, 18872,
            21481, 24412, 27686, 31326, 35355, 39800, 44700, 50098, 56037, 62573
        };

        public int[] GetTable() => Table;

        public int GetRequiredExpUntil(int level)
        {
            int exp = 0;
            if (level > 0)
            {
                exp = Table[level - 1];
            }
            return exp;
        }
    }
}
