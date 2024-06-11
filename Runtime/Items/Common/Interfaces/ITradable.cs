using System;
using UnityEngine;

namespace Glitch9.Game
{
    public interface ITradable
    {
        int Index { get; }
        string Name { get; }
        Rarity Rarity { get; }
        Sprite Icon { get; }
        int BuyPrice { get; set; }
        int SellPrice { get; set; }
        bool CantSell => SellPrice < 1;
        bool CantBuy => BuyPrice < 1;
    }

    public static class TradableExtensions
    {
        public static IResult TryBuy(this ITradable itemToBuy, int buyQuantity, Currency buyWith)
        {
            if (itemToBuy.LogIfNull() || buyWith.LogIfNull() || buyQuantity < 1) return Result.Fail("Invalid trade parameters");
            ItemTrade itemTrade = buyWith.CreateTrade(TradeType.Buy, itemToBuy, buyQuantity);
            return itemTrade.ExecuteTrade();
        }

        public static int SetBuyBatch(this ITradable itemToBuy, int buyQuantity, Currency buyWith, int batchId = -1)
        {
            if (itemToBuy.LogIfNull() || buyWith.LogIfNull() || buyQuantity < 1) return -1;
            ItemTrade itemTrade = buyWith.CreateTrade(TradeType.Buy, itemToBuy, buyQuantity);
            return itemTrade.SetTradeBatch(batchId);
        }

        /// <summary>
        /// 아이템을 살거냐는 경고를 띄운 후 Yes를 누를시 구매한다.
        /// </summary>
        public static IResult TryBuyWithAlert(this ITradable itemToBuy, string msg, int buyQuantity, Currency buyWith)
        {
            if (itemToBuy.LogIfNull() || buyWith.LogIfNull() || buyQuantity < 1) return Result.Fail("Invalid trade parameters");
            ItemTrade itemTrade = buyWith.CreateTrade(TradeType.Buy, itemToBuy, itemToBuy.BuyPrice, buyQuantity);
            return itemTrade.ExecuteTrade(msg);
        }

        public static IResult TryBuyWithAlert(this ITradable item, int buyQuantity, Currency buyWith)
        {
            // TODO : 디폴트 물건 구매 메시지 로컬라이징 + 리파인 필요      
            string msg = "Would you like to buy this item?";
            return TryBuyWithAlert(item, msg, buyQuantity, buyWith);
        }


        public static IResult TrySell(this ITradable itemToSell, int sellQuantity, Currency sellWith)
        {
            if (itemToSell.LogIfNull() || sellWith.LogIfNull() || sellQuantity < 1) return Result.Fail("Invalid trade parameters");
            ItemTrade itemTrade = sellWith.CreateTrade(TradeType.Sell, itemToSell, sellQuantity);
            return itemTrade.ExecuteTrade();
        }

        public static int SetSellBatch(this ITradable itemToSell, int sellQuantity, Currency sellWith, int batchId = -1)
        {
            if (itemToSell.LogIfNull() || sellWith.LogIfNull() || sellQuantity < 1) return -1;
            ItemTrade itemTrade = sellWith.CreateTrade(TradeType.Sell, itemToSell, sellQuantity);
            return itemTrade.SetTradeBatch(batchId);
        }

        /// <summary>
        /// 아이템을 팔거냐는 경고를 띄운 후 Yes를 누를시 판매한다.
        /// </summary>
        public static IResult TrySellWithAlert(this ITradable itemToSell, string msg, int sellQuantity, Currency sellWith)
        {
            if (itemToSell.LogIfNull() || sellWith.LogIfNull() || sellQuantity < 1) return Result.Fail("Invalid trade parameters");
            ItemTrade itemTrade = sellWith.CreateTrade(TradeType.Sell, itemToSell, sellQuantity);
            return itemTrade.ExecuteTrade(msg);
        }

        public static IResult TrySellWithAlert(this ITradable item, int sellQuantity, Currency sellWith)
        {
            // TODO : 디폴트 물건 판매 메시지 로컬라이징 + 리파인 필요         
            string msg = "Would you like to sell this item?";
            return TrySellWithAlert(item, msg, sellQuantity, sellWith);
        }
    }
}