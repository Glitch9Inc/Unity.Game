



namespace Glitch9.Game
{
    public interface ITimeLimited
    {
        public string Id { get; set; }
        public UnixTime StartTime { get; set; }
        public UnixTime EndTime { get; set; }
    }
}
