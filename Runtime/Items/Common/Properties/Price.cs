using System;

namespace Glitch9.Game
{
    [Serializable]
    public struct Price
    {
        public static implicit operator int(Price price) => price.value;
        public static implicit operator Price(int value) => new Price() { value = value };

        public int value;
        public int currencyId;

        // compare with int
        public static bool operator ==(Price price, int value) => price.value == value;
        public static bool operator !=(Price price, int value) => price.value != value;
        public static bool operator >(Price price, int value) => price.value > value;
        public static bool operator <(Price price, int value) => price.value < value;
        public static bool operator >=(Price price, int value) => price.value >= value;
        public static bool operator <=(Price price, int value) => price.value <= value;

        // equals & hashcode
        public readonly override bool Equals(object obj)
        {
            if (obj is Price price)
            {
                return value == price.value;
            }
            return false;
        }
        public readonly override int GetHashCode() => value;
        public readonly int GetDiscountPrice(float discountRate) => (int)(value * discountRate);

        // CompareTo
        public readonly int CompareTo(Price price) => value.CompareTo(price.value);
    }
}
