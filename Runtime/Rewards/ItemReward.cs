using Cysharp.Threading.Tasks;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Cloud;
using Glitch9.DB;
using System;
using UnityEngine;

namespace Glitch9.Game
{
    /// <summary>
    /// Reward that gives an item.
    /// </summary>
    public class ItemReward : Firedata<ItemReward>, IReward
    {
        [CloudData] public string Id { get; private set; }
        [CloudData] public int Quantity { get; set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }

        public ItemReward(string id, string title, Sprite icon, int quantity = 1)
        {
            Id = id;
            Name = title;
            Icon = icon;
            Quantity = quantity;
        }

        public static ItemReward FromItemId(string id, int quantity = 1)
        {
            Item item = ItemDatabase.Get(id);
            if (item.LogIfNull()) return null;
            return new ItemReward(id, item.Name, item.Icon, quantity);
        }

        public static ItemReward FromItemIndex(int index, int quantity = 1)
        {
            Item item = ItemDatabase.Get(index);
            if (item.LogIfNull()) return null;
            return new ItemReward(item.Id, item.Name, item.Icon, quantity);
        }
    }

    public static class ItemRewardExtensions
    {
        public static async UniTask TryClaimAsync(this ItemReward itemReward, Action<IResult> onComplete = null)
        {
            await Inventory.AcquireAsync(itemReward.Id, itemReward.Quantity, onComplete);
        }

        public static Item GetItem(this ItemReward itemReward)
        {
            return ItemDatabase.Get(itemReward.Id);
        }

        public static bool TryGetItem(this ItemReward itemReward, out Item item)
        {
            item = ItemDatabase.Get(itemReward.Id);
            return item != null;
        }
    }
}