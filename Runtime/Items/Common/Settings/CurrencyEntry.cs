using System;

namespace Glitch9.Game
{

    [Serializable]
    public class CurrencyEntry
    {
        public string currencyId;
        public int itemIndex;

        public CurrencyEntry(string currencyId, int itemIndex)
        {
            this.currencyId = currencyId;
            this.itemIndex = itemIndex;
        }
    }
}
