using Glitch9.Game.MailSystem;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public interface IUser
    {
        int Experience { get; set; }
        int Level { get; }
        IDictionary<string, Mail> Mails { get; set; }
        IList<int> ReceivedSystemMails { get; set; }


        // Metadata
        private static class UserMetadataKeys
        {
            internal const string LOGIN_COUNT = "LoginCount";
            internal const string PURCHASE_COUNT = "PurchaseCount";
            internal const string REFERRAL_COUNT = "ReferralCount";
        }
        
        IDictionary<string, object> Metadata { get; set; }

        T GetMetadata<T>(string key)
        {
            if (Metadata?.ContainsKey(key) == true) return (T)Metadata[key];
            return default;
        }

        void SetMetadata<T>(string key, T value)
        {
            Metadata ??= new Dictionary<string, object>();
            Metadata[key] = value;
        }

        int LoginCount
        {
            get => GetMetadata<int>(UserMetadataKeys.LOGIN_COUNT);
            set => SetMetadata(UserMetadataKeys.LOGIN_COUNT, value);
        }

        int PurchaseCount
        {
            get => GetMetadata<int>(UserMetadataKeys.PURCHASE_COUNT);
            set => SetMetadata(UserMetadataKeys.PURCHASE_COUNT, value);
        }

        int ReferralCount
        {
            get => GetMetadata<int>(UserMetadataKeys.REFERRAL_COUNT);
            set => SetMetadata(UserMetadataKeys.REFERRAL_COUNT, value);
        }
    }
}