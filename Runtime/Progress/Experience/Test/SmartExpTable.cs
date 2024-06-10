namespace Glitch9
{
    /// <summary>
    /// 경험치 테이블을 자동으로 생성해주는 클래스
    /// 현재 작업중으로 사용할 수 없음
    /// </summary>
    abstract class SmartExpTable
    {
        /// <summary>
        /// The maximum level of the table.
        /// </summary>
        protected abstract int MaxLvl { get; }

        /// <summary>
        /// The amount of experience required to reach level 2 from level 1.
        /// </summary>
        protected abstract int ExpRequiredToLvl { get; }

        /// <summary>
        /// The amount of experience increased for ExpRequiredToLvl.
        /// If ExpRequiredToLvl is 100 and ExpRequiredIncrementPerLvl is 10, 
        /// then the amount of experience required to reach level 3 from level 2 is 110.
        /// </summary>
        protected abstract int ExpRequiredIncrementPerLvl { get; }

        private int specialThresholdStartingLvl
        {
            get
            {
                if (SpecialThresholdStartingLvl > MaxLvl)
                    return MaxLvl;
                else
                    return SpecialThresholdStartingLvl;
            }
        }

        /// <summary>
        /// The level where the experience requirements might jump or drop significantly.
        /// Use significant values like 50, 80, 90, 99.
        /// This can't be more than MaxLvl.
        /// </summary>
        protected abstract int SpecialThresholdStartingLvl { get; }

        /// <summary>
        /// From SpecialThresholdStartingLvl to MaxLvl, 
        /// the amount of experience required to reach the next level is multiplied by this value.
        /// </summary>
        protected abstract float SpecialThresholdMultiplier { get; }

        /// <summary>
        /// If true, the SpecialThresholdMultiplier is squared.
        /// </summary>
        protected abstract bool SquareSpecialThresholdMultiplier { get; }

        private int[] _cachedTable;
        protected int[] Table
        {
            get
            {
                if (_cachedTable == null)
                {
                    _cachedTable = new int[MaxLvl + 1];
                    _cachedTable[0] = 0;

                    for (int i = 1; i < specialThresholdStartingLvl; i++)
                    {
                        int increment = ExpRequiredIncrementPerLvl * (i - 1);
                        int expRequired = ExpRequiredToLvl + increment;
                        _cachedTable[i] = _cachedTable[i - 1] + expRequired;
                    }

                    for (int i = specialThresholdStartingLvl; i <= MaxLvl; i++)
                    {
                        int increment = ExpRequiredIncrementPerLvl * (i - 1);
                        float multiplier = SpecialThresholdMultiplier;
                        if (SquareSpecialThresholdMultiplier) multiplier *= multiplier;
                        int expRequired = (int)(ExpRequiredToLvl + increment * multiplier);
                        _cachedTable[i] = _cachedTable[i - 1] + expRequired;
                    }
                }

                return _cachedTable;
            }
        }


        public int GetLevel(int exp)
        {
            if (exp <= 0) return 0;

            int level = 0;
            while (exp >= Table[level])
            {
                level++;
                if (level > MaxLvl)
                    break;
            }

            return level - 1;
        }
    }
}
