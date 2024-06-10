namespace Glitch9.Game
{
    public static class RewardUtils
    {
        public static int CalculateRequiredProgress(int tier, int requiredCount, IncrementMethod method, int increment)
        {
            if (method == IncrementMethod.None) return requiredCount;
            tier += 1; // 0부터 시작하므로 1을 더해준다.

            return method switch
            {
                IncrementMethod.Add => requiredCount + increment * tier,
                IncrementMethod.Multiply => requiredCount * increment * tier,
                _ => requiredCount,
            };
        }

        public static RewardData IncreaseRewards(int tier, RewardData rewardData, IncrementMethod method, double increment)
        {
            if (method == IncrementMethod.None) return rewardData;
            tier += 1; // 0부터 시작하므로 1을 더해준다.

            RewardData newReward = new()
            {
                ExpReward = rewardData.ExpReward,
                SeasonExpReward = rewardData.SeasonExpReward
            };

            if (rewardData.Rewards.IsValid())
            {
                foreach (IReward r in rewardData.Rewards)
                {
                    if (r is ItemReward itemReward)
                    {
                        int newQuantity = 0;

                        if (method == IncrementMethod.Add)
                        {
                            newQuantity = (int)(itemReward.Quantity * increment * tier);
                        }
                        else if (method == IncrementMethod.Multiply)
                        {
                            newQuantity = (int)(itemReward.Quantity * increment * tier);
                        }

                        newReward.Add(ItemReward.FromItemId(itemReward.Id, newQuantity));
                    }
                }
            }

            return newReward;
        }

        public static IReward CreateReward(string spreadsheetField)
        {
            string[] parts = spreadsheetField.Split(':');

            if (parts.Length == 2)
            {
                string key = parts[0];
                string value = parts[1];
                if (int.TryParse(value, out int quantity))
                {
                    return ItemReward.FromItemId(key, quantity);
                }

                return null;
            }
            
            return null;
        }
    }
}