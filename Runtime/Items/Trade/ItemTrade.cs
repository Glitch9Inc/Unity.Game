namespace Glitch9.Game
{
    public class ItemTrade
    {
        public TradeType TradeType { get; set; }
        public Item ToRemove { get; set; }
        public Item ToAdd { get; set; }
        /// <summary>
        /// 구입하고자 하는 아이템의 수량
        /// </summary>
        public int QuantityToAdd { get; set; }
        /// <summary>
        /// 구입하고자 하는 아이템의 가격
        /// </summary>
        public int PricePerItem { get; set; }
        /// <summary>
        /// 구입하고자 하는 아이템의 총 가격
        /// </summary>
        public int QuantityToRemove => QuantityToAdd * PricePerItem;
        /// <summary>
        /// 유저가 현재 가지고 있는 자원의 수량
        /// </summary>
        public int UserHasEnoughSource => Inventory.GetOwnQuantity(ToRemove.Id);
        /// <summary>
        /// 구입 가능 여부
        /// </summary>
        public bool CanBuy => UserHasEnoughSource >= QuantityToRemove;

        public string AlertMessage { get; set; }
    }
}