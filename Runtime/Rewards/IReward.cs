
using UnityEngine;

namespace Glitch9.Game
{
    public interface IReward 
    {
        string Id { get; }
        string Name { get; }
        Sprite Icon { get; }
        int Quantity { get; }
    }
}