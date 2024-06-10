using Cysharp.Threading.Tasks;
using Glitch9.Apis.Google.Firestore;
using Glitch9.DB;
using System;

namespace Glitch9.Game
{
    public class ConsumableItem : Item, IConsumable
    {
        public ConsumableType Type { get; set; }
        public int GetAmount { get; set; }

        public async UniTask<IResult> ConsumeAsync()
        {
            switch (Type)
            {
                case ConsumableType.GetRandomItem: return await HandleRandomBox();
                case ConsumableType.Function: return await HandleFunction();
            }

            return Result.Fail($"Unsupported consumable type: {Type}");
        }

        private async UniTask<IResult> HandleRandomBox()
        {
            // Args에 1번이 있는지 확인
            if (Arguments.Length < 2) return Result.Fail("Invalid arguments");

            if (int.TryParse(Arguments[1], out int poolId))
            {
                IItemPool pool = ItemPoolRegistry.Get(poolId);
                if (pool == null) return Result.Fail("Invalid pool id");

                ItemReward[] itemRewards = pool.DrawItems();
                if (itemRewards.IsNullOrEmpty()) return Result.Fail("Failed to draw items");

                int batchId = DateTimeId.CreateNew();
                foreach (ItemReward itemReward in itemRewards)
                {
                    if (itemReward == null) continue;
                    int newBatchId = Inventory.SetAcquireBatch(itemReward.Id, itemReward.Quantity, batchId);
                    if (newBatchId <= 0) return Result.Fail("Failed to set acquire batch");
                }

                return await HandleResultAsync(batchId);
            }

            return Result.Fail("Invalid pool id");
        }

        private async UniTask<IResult> HandleFunction()
        {
            if (Arguments.Length < 2) return Result.Fail("Invalid arguments");

            try
            {
                // 스프레드시트에서 Argument 1번(0번없음)가 함수명이다.
                // 스프레드시트에서 Argument 2번이 파라미터이다.
                string funcName = Arguments[0];
                if (string.IsNullOrEmpty(funcName)) return Result.Fail("Invalid function name");
                if (!int.TryParse(Arguments[1], out int parameter)) return Result.Fail("Invalid parameter");
                IResult result = ConsumeFunctionRegistry.Execute(funcName, parameter);
                if (result is Result<int> resultWithBatchId) return await HandleResultAsync(resultWithBatchId.Value);
                return Result.Error(Issue.UnknownError);
            }
            catch (Exception e)
            {
                GNLog.Exception(e);
                GameManager.Instance.DisplayIssue(this, Issue.UnknownError);
            }

            return Result.Fail("Failed to execute function");
        }

        private async UniTask<IResult> HandleResultAsync(int batchId)
        {
            if (batchId <= 0) return Result.Fail("Invalid batch id");

            int newBatchId = this.SetRemoveBatch(1, batchId);
            if (newBatchId <= 0) return Result.Fail("Invalid batch id");

            return await Firetask.ExecuteBatchAsync(newBatchId);
        }
    }
}
