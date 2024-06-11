namespace Glitch9.Game
{
    public static class ItemTradeExtensions
    {
        public static ItemTrade CreateTrade(this Item source, int price, int quantity = 1)
        {
            return new ItemTrade()
            {
                TradeType = TradeType.Pay,
                ToRemove = source,
                QuantityToAdd = quantity,
                PricePerItem = price,
            };
        }

        public static ItemTrade CreateTrade(this Item source, TradeType tradeType, ITradable target, int price, int quantity = 1)
        {
            return new ItemTrade()
            {
                TradeType = tradeType,
                ToRemove = target as Item,
                ToAdd = source,
                QuantityToAdd = quantity,
                PricePerItem = price,
            };
        }

        public static IResult ExecuteTrade(this ItemTrade itemTrade, string alertMessage = null)
        {
            if (itemTrade.CanBuy)
            {
                if (string.IsNullOrEmpty(alertMessage))
                {
                    MyGame.ExecuteTrade(nameof(ItemTradeExtensions), itemTrade);
                }
                else
                {
                    itemTrade.AlertMessage = alertMessage;
                    MyGame.ExecuteTrade(nameof(ItemTradeExtensions), itemTrade);
                }
            }
            return Result.Error(Issue.InsufficientCurrency);
        }


        public static int SetTradeBatch(this ItemTrade itemTrade, int batchId = -1)
        {
            if (itemTrade.TradeType != TradeType.Pay)
            {
                Item itemToAdd = itemTrade.ToAdd;
                batchId = itemToAdd.SetAcquireBatch(itemTrade.QuantityToAdd, batchId);
            }

            Item itemToRemove = itemTrade.ToRemove;
            batchId = itemToRemove.SetRemoveBatch(itemTrade.QuantityToRemove, batchId);

            return batchId;
        }
    }
}