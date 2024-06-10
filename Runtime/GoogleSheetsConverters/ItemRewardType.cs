using Glitch9.Game;

namespace Glitch9.Apis.Google.Sheets
{
    [Type(typeof(ItemReward), new string[] { "Stock", "Item", "ItemStock", "ItemReward" })]
    public class ItemRewardType : IType
    {
        public object DefaultValue => ItemReward.FromItemId("credit", 0);

        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value)) return DefaultValue;
            var values = value.Trim('[', ']').Split(':');

            if (values.Length == 2)
            {
                string id = values[0];
                int amount = int.Parse(values[1]);
                return ItemReward.FromItemId(id, amount);
            }
            else if (values.Length == 1)
            {
                string id = values[0];
                return ItemReward.FromItemId(id);
            }
            else
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// value write to google sheet
        /// </summary> 
        public string Write(object value)
        {
            ItemReward v = (ItemReward)value;
            return $"{v.Id}:{v.Quantity}";
        }
    }
}