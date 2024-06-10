using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    [Serializable]
    public class ComboCounter
    {
        public int ComboCount { get; private set; } = 0;
        public int MaxComboCount { get; private set; } = 0;
        public int ComboScore { get; private set; } = 0;
        public int MaxComboScore { get; private set; } = 0;
        public int ComboStage { get; private set; }

        public HashSet<int> ComboStageSet;
        public IntValueChange ComboScoreChange;
        public event Action<int> OnComboCountChanged;

        public void AddCombo(int score)
        {
            ComboCount++;
            ComboScore += score;
            if (ComboCount > MaxComboCount) MaxComboCount = ComboCount;
            if (ComboScore > MaxComboScore) MaxComboScore = ComboScore;

            // check if combostage set contains current combo count
            if (ComboStageSet != null && ComboStageSet.Contains(ComboCount))
            {
                ComboStage++;
            }

            OnComboCountChanged?.Invoke(ComboCount);
        }

        public void ResetCombo()
        {
            ComboCount = 0;
            ComboScore = 0;
        }
    }
}
