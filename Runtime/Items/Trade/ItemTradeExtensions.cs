using System;
using Glitch9.Apis.Google.Firestore;

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
                    GameManager.Instance.EventHandler.OnItemTrade?.Invoke(nameof(ItemTradeExtensions), itemTrade);
                }
                else
                {
                    itemTrade.AlertMessage = alertMessage;
                    GameManager.Instance.EventHandler.OnItemTrade?.Invoke(nameof(ItemTradeExtensions), itemTrade);
                }
            }
            return Result.Error(Issue.InsufficientCurrency);
        }

        public static void ExecuteTrade(this ItemTrade itemTrade, Action<IResult> onResult = null)
        {
            int batchId = itemTrade.SetTradeBatch();
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onResult?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            Firetask.ExecuteBatch(batchId, onResult);
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