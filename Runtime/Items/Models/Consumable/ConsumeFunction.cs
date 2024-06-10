using System;

namespace Glitch9.Game
{
    public class ConsumeFunction
    {
        private const string FALLBACK_CONSUME_FAIL_MESSAGE = "You can't use {0} right now.";
        public string itemId;
        public Func<int, Result<int>> function;
        public Func<string, bool> condition;
        public string conditionFailMessage;

        public ConsumeFunction(string itemId, Func<int, Result<int>> function, Func<string, bool> condition = null, string conditionFailMessage = null)
        {
            this.itemId = itemId;
            this.function = function;
            this.condition = condition;
            this.conditionFailMessage = conditionFailMessage;
        }

        public bool CheckCondition()
        {
            return condition == null || condition(itemId);
        }

        public IResult Execute(int parameter)
        {
            if (!CheckCondition())
            {
                return Result.Fail(string.Format(conditionFailMessage ?? FALLBACK_CONSUME_FAIL_MESSAGE, itemId));
            }

            return function(parameter);
        }
    }
}