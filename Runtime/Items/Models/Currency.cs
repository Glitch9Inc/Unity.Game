using System;
using Glitch9.DB;

namespace Glitch9.Game
{
    public class Currency : Item
    {
        public CurrencyType Type { get; private set; }
        public string FreeId { get; private set; }
        public string PremiumId { get; private set; }

        // Constructor for Free currency
        public Currency(CurrencyType type)
        {
            Type = type;

            if (type == CurrencyType.Premium)
            {
                throw new ArgumentException("For premium currency, please use the constructor that includes freeId and premiumId.");
            }
        }

        // Constructor for Premium currency
        public Currency(string freeId, string premiumId)
        {
            Type = CurrencyType.Premium;
            FreeId = freeId;
            PremiumId = premiumId;
        }

        public Currency() { }

        public override int Quantity
        {
            get
            {
                return Type switch
                {
                    CurrencyType.Free => Inventory.GetOwnQuantity(GetId()),
                    CurrencyType.Premium => Inventory.GetOwnQuantity(FreeId) + Inventory.GetOwnQuantity(PremiumId),
                    _ => throw new InvalidOperationException("Unknown currency type."),
                };
            }
            set => throw new Exception($"{Type} Currency Quantity is managed internally.");
        }

        public static implicit operator int(Currency value) => value.Quantity;

        public bool HasEnough(int value) => Quantity >= value;

        public string GetId()
        {
            if (Type == CurrencyType.Free)
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return ItemDatabase.GetId(Index);
                }
                return Id;
            }
            else
            {
                throw new InvalidOperationException("Premium currency should use FreeId or PremiumId.");
            }
        }

        public void AddBatch(int value, int batchId, Action<IResult> onComplete = null)
        {
            if (Type == CurrencyType.Premium)
            {
                throw new InvalidOperationException("Use AddBatchFree or AddBatchPremium for premium currencies.");
            }

            Inventory.SetQuantityBatch(GetId(), value, batchId, onComplete);
        }

        public void AddBatchFree(int value, int batchId, Action<IResult> onComplete = null) => Inventory.SetQuantityBatch(FreeId, value, batchId, onComplete);
        public void AddBatchPremium(int value, int batchId, Action<IResult> onComplete = null) => Inventory.SetQuantityBatch(PremiumId, value, batchId, onComplete);

        public int RemoveQuantityBatch(int value, int batchId = -1, Action<IResult> onComplete = null)
        {
            if (Type == CurrencyType.Free)
            {
                return Inventory.SetQuantityBatch(GetId(), -value, batchId, onComplete);
            }
            else
            {
                int free = Inventory.GetOwnQuantity(FreeId);
                int paid = Inventory.GetOwnQuantity(PremiumId);
                bool freeIsEnough = free >= value;

                if (freeIsEnough)
                {
                    return Inventory.SetQuantityBatch(FreeId, -value, batchId, onComplete);
                }
                else
                {
                    int newBatchId = Inventory.SetQuantityBatch(FreeId, -free, batchId, onComplete);
                    return Inventory.SetQuantityBatch(PremiumId, -(value - free), newBatchId, onComplete);
                }
            }
        }
    }
}