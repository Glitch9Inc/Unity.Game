using Cysharp.Threading.Tasks;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Apis.Google.Firestore.Tasks;
using Glitch9.Database;
using Glitch9.DB;
using System;

namespace Glitch9.Game
{
    public class Inventory
    {
        public static FirestoreDictionary<ItemData> CloudInstance { get; private set; }

        public static void Initialize(params string[] cloudIds)
        {
            CloudInstance = new FirestoreDictionary<ItemData>(cloudIds);
        }

        public static ItemData GetPlayerData(string itemId, bool ensure = false)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                GNLog.Error("잘못된 아이템 아이디: " + itemId);
                return null;
            }
            if (CloudInstance.LogIfNull()) throw new ArgumentNullException(nameof(CloudInstance));
            if (CloudInstance.TryGetValue(itemId, out ItemData item)) return item;
            if (ensure)
            {
                item = new ItemData
                {
                    Id = itemId,
                    Quantity = 0,
                };
                CloudInstance.Add(itemId, item);
                return item;
            }
            return null;
        }
        public static int GetOwnQuantity(string itemId) => GetPlayerData(itemId)?.Quantity ?? 0;
        public static bool Has(string itemId, int quantity = 1)
        {
            if (quantity < 1)
            {
                GNLog.Error($"잘못된 아이템 수량: {quantity}");
                return false;
            }
            return GetOwnQuantity(itemId) >= quantity;
        }
        public static bool Has(int itemIndex, int quantity = 1)
        {
            string itemId = ItemDatabase.Get(itemIndex)?.Id ?? string.Empty;
            return Has(itemId, quantity);
        }

        public static void SetQuantityLocal(string itemId, int quantity)
        {
            ItemData playerData = GetPlayerData(itemId, true); // 없으면 새로운 인스턴스를 생성한다. 

            if (quantity < 1)
            {
                CloudInstance.Remove(itemId);
                return;
            }

            playerData.Quantity = quantity;
        }

        public static void AddQuantityLocal(string itemId, int quantity) => SetQuantityLocal(itemId, GetOwnQuantity(itemId) + quantity);
        public static void RemoveQuantityLocal(string itemId, int quantity) => SetQuantityLocal(itemId, GetOwnQuantity(itemId) - quantity);

        public static async UniTask AcquireAsync(int index, int quantity = 1, Action<IResult> onComplete = null)
        {
            int batchId = SetAcquireBatch(ItemDatabase.Get(index), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static async UniTask AcquireAsync(string itemId, int quantity = 1, Action<IResult> onComplete = null)
        {
            int batchId = SetAcquireBatch(ItemDatabase.Get(itemId), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static async UniTask RemoveAsync(int index, int quantity = 1, Action<IResult> onComplete = null)
        {
            int batchId = SetRemoveBatch(ItemDatabase.Get(index), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static async UniTask RemoveAsync(string itemId, int quantity = 1, Action<IResult> onComplete = null)
        {
            int batchId = SetRemoveBatch(ItemDatabase.Get(itemId), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static async UniTask SetQuantityAsync(int index, int quantity, Action<IResult> onComplete = null)
        {
            int batchId = SetQuantityBatch(ItemDatabase.Get(index), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static async UniTask SetQuantityAsync(string itemId, int quantity, Action<IResult> onComplete = null)
        {
            int batchId = SetQuantityBatch(ItemDatabase.Get(itemId), quantity, -1, onComplete);
            if (batchId == -1)
            {
                GNLog.Error(Issue.InvalidBatchOperation);
                onComplete?.Invoke(Result.Error(Issue.InvalidBatchOperation));
                return;
            }
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        public static int SetQuantityBatch(int index, int quantity, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetQuantityBatch(ItemDatabase.Get(index), quantity, batchId, onComplete);
        }

        public static int SetQuantityBatch(string itemId, int quantity, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetQuantityBatch(ItemDatabase.Get(itemId), quantity, batchId, onComplete);
        }

        public static int SetQuantityBatch(Item item, int quantity, int batchId = -1, Action<IResult> onComplete = null)
        {
            ItemData save = ValidateTaskAndGetPlayerData(item, quantity);
            if (save.LogIfNull()) return -1;
            save.Quantity = quantity;

            FieldTask fieldTask = new(CloudInstance.Document);

            if (quantity == 0)
            {
                fieldTask.DeleteData(item.Id);
                fieldTask.OnComplete = (success) => OnServerResult(success, DatabaseAction.Remove, item.Id, quantity, item is Currency, onComplete);
            }
            else
            {
                fieldTask.SetData(item.Id, save);
                fieldTask.OnComplete = (success) => OnServerResult(success, DatabaseAction.Set, item.Id, quantity, item is Currency, onComplete);
            }

            return fieldTask.SetBatch(batchId);
        }

        public static int SetAcquireBatch(int index, int quantityToAdd = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetAcquireBatch(ItemDatabase.Get(index), quantityToAdd, batchId, onComplete);
        }

        public static int SetAcquireBatch(string itemId, int quantityToAdd = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetAcquireBatch(ItemDatabase.Get(itemId), quantityToAdd, batchId, onComplete);
        }

        public static int SetAcquireBatch(Item item, int quantityToAdd = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            ItemData save = ValidateTaskAndGetPlayerData(item, quantityToAdd);
            if (save.LogIfNull()) return -1;
            save.Quantity += quantityToAdd;

            FieldTask fieldTask = new(CloudInstance.Document);
            fieldTask.SetData(item.Id, save);
            fieldTask.OnComplete = (success) => OnServerResult(success, DatabaseAction.Add, item.Id, quantityToAdd, item is Currency, onComplete);
            return fieldTask.SetBatch(batchId);
        }

        public static int SetRemoveBatch(int index, int quantityToRemove = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetRemoveBatch(ItemDatabase.Get(index), quantityToRemove, batchId, onComplete);
        }

        public static int SetRemoveBatch(string itemId, int quantityToRemove = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            return SetRemoveBatch(ItemDatabase.Get(itemId), quantityToRemove, batchId, onComplete);
        }

        public static int SetRemoveBatch(Item item, int quantityToRemove = 1, int batchId = -1, Action<IResult> onComplete = null)
        {
            ItemData save = ValidateTaskAndGetPlayerData(item, quantityToRemove);
            if (save.LogIfNull()) return -1;
            save.Quantity -= quantityToRemove;

            if (save.Quantity < 0)
            {
                GNLog.Warning($"아이템 수량 부족: {item.Index} has {GetOwnQuantity(item.Id)} but tried to remove {quantityToRemove}");
                onComplete?.Invoke(Result.Error(Issue.InvalidAmount));
                return -1;
            }

            FieldTask fieldTask = new(CloudInstance.Document);

            if (save.Quantity == 0)
            {
                fieldTask.DeleteData(item.Id);
                fieldTask.OnComplete = (result) => OnServerResult(result, DatabaseAction.Remove, item.Id, quantityToRemove, item is Currency, onComplete);
            }
            else
            {
                fieldTask.SetData(item.Id, save);
                fieldTask.OnComplete = (result) => OnServerResult(result, DatabaseAction.Remove, item.Id, quantityToRemove, item is Currency, onComplete);
            }

            return fieldTask.SetBatch(batchId);
        }

        private static ItemData ValidateTaskAndGetPlayerData(Item item, int quantityChange)
        {
            if (item.LogIfNull()) return null;
            if (quantityChange < 1)
            {
                GNLog.Error($"잘못된 아이템 수량: {quantityChange}");
                return null;
            }
            return GetPlayerData(item.Id, true);
        }

        private static void OnServerResult(IResult result, DatabaseAction action, string itemId, int chgValue, bool isCurrency, Action<IResult> onComplete)
        {
            if (action == DatabaseAction.Unset)
            {
                GNLog.Error("잘못된 인벤토리 작업타입: Unset");
                onComplete?.Invoke(Result.Error(Issue.InvalidOperation));
                return;
            }

            if (result.IsSuccess)
            {
                if (isCurrency)
                {
                    MyGame.UpdateCurrency(nameof(Inventory));
                }

                if (action == DatabaseAction.Add)
                {
                    AddQuantityLocal(itemId, chgValue);
                }
                else if (action == DatabaseAction.Remove)
                {
                    RemoveQuantityLocal(itemId, chgValue);
                }

            }

            onComplete?.Invoke(result);
        }
    }
}