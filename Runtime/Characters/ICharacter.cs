namespace Glitch9.Game
{
    public interface ICharacter
    {
        string Id { get; }
        string Name { get; }
        bool IsSpawned { get; set; }
        void Save();
    }
}