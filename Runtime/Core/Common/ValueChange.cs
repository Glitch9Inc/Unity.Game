using System;

namespace Glitch9
{
    [Serializable]
    public class ValueChange<T>
    {
        public T PositiveChange;
        public T NeutralChange;
        public T NegativeChange;

        public ValueChange(T positiveChange, T neutralChange, T negativeChange)
        {
            PositiveChange = positiveChange;
            NeutralChange = neutralChange;
            NegativeChange = negativeChange;
        }
    }

    [Serializable]
    public class FloatValueChange : ValueChange<float>
    {
        public FloatValueChange(float positiveChange, float neutralChange, float negativeChange)
            : base(positiveChange, neutralChange, negativeChange)
        {
        }
    }

    [Serializable]
    public class IntValueChange : ValueChange<int>
    {
        public IntValueChange(int positiveChange, int neutralChange, int negativeChange)
            : base(positiveChange, neutralChange, negativeChange)
        {
        }
    }
}
