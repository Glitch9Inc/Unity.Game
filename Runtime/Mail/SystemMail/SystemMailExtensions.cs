using System.Collections.Generic;
using System.Linq;

namespace Glitch9.Game.MailSystem
{
    public static class SystemMailExtensions
    {
        private const string SENDER_NAME = "Glitch9";

        public static Mail ToMail(this SystemMail systemMail)
        {
            Mail mail = new()
            {
                Id = systemMail.Index,
                Title = systemMail.Subject,
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
    }
}