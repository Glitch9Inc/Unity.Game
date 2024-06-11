namespace Glitch9.Game
{
    public interface ICharacterPrefab
    {
        string Id { get; }
        string Name { get; }
        ICharacter Character { get; }

        void Show();
        void Hide();
    }
}