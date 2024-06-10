using System;
using UnityEngine;

namespace Glitch9.Game
{
    [Serializable]
    public class Rarity
    {
        public int value;
        public Color color;

        public static implicit operator int(Rarity rarity) => rarity.value;
        public static implicit operator Rarity(int value) => ItemSettings.GetRarity(value);

        // compare with int
        public static bool operator ==(Rarity rarity, int value) => rarity.value == value;
        public static bool operator !=(Rarity rarity, int value) => rarity.value != value;
        public static bool operator >(Rarity rarity, int value) => rarity.value > value;
        public static bool operator <(Rarity rarity, int value) => rarity.value < value;
        public static bool operator >=(Rarity rarity, int value) => rarity.value >= value;
        public static bool operator <=(Rarity rarity, int value) => rarity.value <= value;



        // equals & hashcode
        public override bool Equals(object obj)
        {
            if (obj is Rarity rarity)
            {
                return value == rarity.value;
            }
            return false;
        }
        public override int GetHashCode() => value;

        // CompareTo
        public int CompareTo(Rarity rarity) => value.CompareTo(rarity.value);
        public override string ToString()
        {
            return value switch
            {
                0 => "Common",
                1 => "Uncommon",
                2 => "Rare",
                3 => "Epic",
                4 => "Legendary",
                _ => "Unknown",
            };

        }
    }
}
