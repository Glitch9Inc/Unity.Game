using Glitch9.Apis.Google.Firestore;
using Glitch9.Cloud;



namespace Glitch9
{
    public enum FriendStatus
    {
        None,
        Requested,
        Accepted,
        Blocked
    }

    public class Friend : Firedata<Friend>, IModel, IMapEntry
    {
        public string Key => Email;
        [CloudData] public string Email { get; set; } = "";
        [CloudData] public FriendStatus Status { get; set; } = FriendStatus.None;
        [CloudData] public UnixTime FriendRequestDate { get; set; }
        [CloudData] public UnixTime FriendAcceptDate { get; set; }
        [CloudData] public UnixTime FriendBlockDate { get; set; }

        public Friend() : base() { }
        public Friend(string email) : base(email)
        {
            Email = email;
        }
    }
}
