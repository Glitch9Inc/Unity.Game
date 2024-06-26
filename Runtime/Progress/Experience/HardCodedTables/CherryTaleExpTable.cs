
namespace Glitch9.Game.HardCodedTables
{
    public class CherryTaleExpTable : IExpTable
    {
        // 최대 50레벨
        readonly int[] Table =
        {
            0, 20, 60, 260, 700, 1340, 2180, 3220, 4460, 5900,
            7540, 9380, 11420, 13660, 16100, 18740, 21580, 24620, 27860, 31300,
            34940, 38780, 42820, 47060, 51500, 56140, 60980, 66020, 71260, 76700,
            82340, 88180, 94220, 100660, 107100, 113540, 120980, 128420, 135860, 143300,
            150740, 158180, 165620, 173060, 180500, 187940, 195380, 202820, 210260, 217700,
        };

        public int[] GetTable() => Table;
    }
}
