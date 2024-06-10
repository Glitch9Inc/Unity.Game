using Cysharp.Threading.Tasks;

namespace Glitch9.Game
{
    public interface IConsumable
    {
        UniTask<IResult> ConsumeAsync();
    }
}
