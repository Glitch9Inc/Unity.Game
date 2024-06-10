namespace Glitch9.Game.HardCodedTables
{
    public class AffectionPtTable : IExpTable
    {
        // 호감도 레벨 1 ~ 6
        readonly int[] Table =
        {
            0, 100, 200, 300, 400, 800 //800 이상 결혼 가능
        };

        public int[] GetTable() => Table;
    }
}
