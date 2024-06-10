using System.Collections.Generic;

namespace Glitch9.Game
{
    public class ConsumeFunctionRegistry
    {

        private static readonly Dictionary<string, ConsumeFunction> _functions = new();

        public static void Register(ConsumeFunction function)
        {
            _functions[function.itemId] = function;
        }

        public static IResult Execute(string itemId, int parameter)
        {
            if (!_functions.TryGetValue(itemId, out ConsumeFunction function))
            {
                return Result<int>.Fail(-1, "Invalid item id");
            }

            return function.Execute(parameter);
        }
    }
}