using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using Glitch9.Apis.Google.Firebase;
using Glitch9.Apis.Google.Firestore;

namespace Glitch9.Game.MailSystem
{
    public static class SystemMailUtils
    {
        private const string SENDER_NAME = "Glitch9";

        public static Mail ToMail(this SystemMail systemMail)
        {
            Mail mail = new()
            {
                Id = systemMail.Index,
                Subject = systemMail.Subject,
                Content = systemMail.Content,
                Attachments = ParseAttachedItems(systemMail.Attachments).ToList(),
                Type = systemMail.Condition,
                Sender = SENDER_NAME
            };
            return mail;
        }

        public static IEnumerable<ItemReward> ParseAttachedItems(string attachment)
        {
            if (string.IsNullOrEmpty(attachment)) yield break;
            // 서버 데이터 형식 : string / credit: 5000,crystal_free: 3000
            string[] items = attachment.Split(',');

            foreach (string item in items)
            {
                string[] keyValue = item.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    int value = int.Parse(keyValue[1]);
                    yield return ItemReward.FromItemId(key, value);
                }
            }
        }

        public static async void SendMailToAll(Mail mail)
        {
            if (FirebaseManager.User == null)
            {
                GNLog.Error("먼저 Firebase에 로그인하세요.");
                return;
            }

            QuerySnapshot allUserDatasQuerySnapshot = await FirestoreReferences.Users.Root.GetSnapshotAsync();

            foreach (DocumentSnapshot userData in allUserDatasQuerySnapshot.Documents)
            {
                GNLog.Info("<color=blue>전체 메일을 보냈습니다</color> : " + userData.Id.ToString());
                mail.Send(email: userData.Id);
            }
        }
    }
}