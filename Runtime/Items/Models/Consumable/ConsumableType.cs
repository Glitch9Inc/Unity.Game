namespace Glitch9.Game
{
    public enum ConsumableType
    {
        Unset,
        GetCurrency,   // 재화를 획득하는 아이템
        GainExp,        // 경험치를 획득하는 아이템
        GetRandomItem,     // 랜덤하게 아이템을 획득하는 아이템
        Function,       // 그외 특수한 기능을 실행하는 아이템
    }
}