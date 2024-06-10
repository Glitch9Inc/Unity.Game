
namespace Glitch9.Game
{
    public class Exp<T> where T : IExpTable, new()
    {
        private readonly T _tableInstance = new T();
        public Exp() { }
        public Exp(int value) => Value = value;

        public static implicit operator int(Exp<T> value) => value.Value;
        public static implicit operator Exp<T>(int value) => new Exp<T>(value);

        public int[] Table => _tableInstance.GetTable();

        public int Value;

        public int Level
        {
            get
            {
                int level = 0;
                for (int i = 0; i < Table.Length; i++)
                {
                    if (Value < Table[i])
                    {
                        level = i;
                        break;
                    }
                }
                return level;
            }
        }

        public int GetRequiredExpUntil(int level)
        {
            return Table[level];
        }

        public float GetFillAmount()
        {
            float fillAmount = 0;
            int level = Level;
            if (level > 0)
            {
                int requiredExp = GetRequiredExpUntil(level);
                int nextLevelExp = Table[level];
                fillAmount = (float)(Value - requiredExp) / (float)(nextLevelExp - requiredExp);
            }
            return fillAmount;
        }

        public int RequiredExp => GetRequiredExpUntil(Level);
    }
}
