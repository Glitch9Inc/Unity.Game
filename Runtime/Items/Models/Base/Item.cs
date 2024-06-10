using Glitch9.Database;
using Glitch9.Toolkits.SmartLocalization;
using System;
using UnityEngine;

namespace Glitch9.Game
{
    public class Item : IModel, IListEntry
    {
        // Caching
        private bool? _hideFromInventory;
        public bool HideFromInventory
        {
            get
            {
                _hideFromInventory ??= ItemSettings.GetHideFromInventory(nameof(Item));
                return _hideFromInventory.Value;
            }
        }

        // Immutable properties
        public virtual int Index { get; set; }
        public virtual string Id { get; protected set; }
        public string Name => Id.Localize();
        public string Desc => Id.Localize(Suffix.DESC);
        public string ItemType { get; protected set; }
        public virtual Rarity Rarity { get; protected set; }
        public virtual int IconId { get; protected set; }
        public virtual string PoolId { get; protected set; }
        public Sprite Icon => Sprites.Get(IconId);
        public string[] Arguments { get; protected set; }
        public bool IsAvailable { get; protected set; }

        // Save properties
        private ItemData _save;
        public ItemData save => _save ??= Inventory.GetPlayerData(Id);
        public UnixTime AcquiredAt => save.AcquiredAt;
        public virtual int Quantity
        {
            get => save.Quantity;
            set => save.Quantity = value;
        }

        public void Initialize(int index, string id, bool isAvailable, string itemType, Rarity rarity, int iconId, string poolId, string[] args)
        {
            Index = index;
            Id = id;
            IsAvailable = isAvailable;
            ItemType = itemType;
            Rarity = rarity;
            IconId = iconId;
            PoolId = poolId;
            Arguments = args;
        }

        public Item() { }

        protected T ParseItemType<T>() where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(ItemType)) return default;
            return (T)Enum.Parse(typeof(T), ItemType);
        }
    }

    public static class ItemExtensions
    {
        public static int SetAcquireBatch(this Item item, int quantityToAdd, int batchId = -1, Action<IResult> onComplete = null)
        {
            return Inventory.SetAcquireBatch(item.Index, quantityToAdd, batchId, onComplete);
        }

        public static int SetRemoveBatch(this Item item, int quantityToRemove, int batchId = -1, Action<IResult> onComplete = null)
        {
            return Inventory.SetRemoveBatch(item.Index, quantityToRemove, batchId, onComplete);
        }

        public static int SetQuantityBatch(this Item item, int quantity, int batchId = -1, Action<IResult> onComplete = null)
        {
            return Inventory.SetQuantityBatch(item.Index, quantity, batchId, onComplete);
        }
    }
}
