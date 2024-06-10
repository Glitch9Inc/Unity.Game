using System;
using System.Collections.Generic;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Cloud;

namespace Glitch9.Game.MailSystem
{
    public class SystemMail : FirestoreDocument<SystemMail>
    {
        public const string SUBJECT_FORMAT = "system_mail_subject_{0}";
        public const string CONTENT_FORMAT = "system_mail_content_{0}";
        public override string Key => $"{Index:D4}|{Id}"; // ex. 0001|WelcomeGift

        [CloudData] public int Index { get; set; } = -1;
        [CloudData] public SystemMailCondition Condition { get; set; } = SystemMailCondition.None;
        [CloudData] public string Id { get; set; } = "";
        [CloudData] public string Attachments { get; set; } = ""; // format : "2:3000,1:30000" 아이템ID:개수
        [CloudData] public string Argument { get; set; } = "";

        public string Subject => string.Format(SUBJECT_FORMAT, Index.ToString("D4")); // TODO : Localization
        public string Content => string.Format(CONTENT_FORMAT, Index.ToString("D4")); // TODO : Localization


        public bool IsAvailable()
        {
            if (string.IsNullOrEmpty(Argument)) return false;

            switch (Condition)
            {
                case SystemMailCondition.PlayerLevel:
                    if (int.TryParse(Argument, out int argLevel))
                    {
                        return User.Player.Level >= argLevel;
                    }
                    break;
                case SystemMailCondition.LoginCount:
                    if (int.TryParse(Argument, out int loginCount))
                    {
                        return User.ClientData.TotalLoginCount >= loginCount;
                    }
                    break;
                case SystemMailCondition.PurchaseCount:
                    if (int.TryParse(Argument, out int purchaseCount))
                    {
                        return User.ClientData.TotalPurchaseCount >= purchaseCount;
                    }
                    break;
                case SystemMailCondition.SeasonalEvent:
                    if (Argument.Contains('~')) // '~'가 포함된 문자열을 파싱하여 날짜로 변환
                    {
                        string[] args = Argument.Split('~');
                        if (args.Length != 2) return false;
                        if (DateTime.TryParse(args[0], out DateTime argDate1) && DateTime.TryParse(args[1], out DateTime argDate2))
                        {
                            return DateTime.Now >= argDate1 && DateTime.Now <= argDate2;
                        }
                    }
                    break;
                case SystemMailCondition.AchievementCompletion: // 사용하지 않는 조건                    
                    GNLog.Error("'AchievementCompletion'은 사용하지 않는 시스템 메일 조건입니다.");
                    break;
                case SystemMailCondition.FriendReferralCount:
                    if (int.TryParse(Argument, out int referralCount))
                    {
                        return User.ClientData.ReferralCount >= referralCount;
                    }
                    break;
                case SystemMailCondition.SpecialDate:
                    if (DateTime.TryParse(Argument, out DateTime argDate))
                    {
                        return DateTime.Now.Month == argDate.Month && DateTime.Now.Day == argDate.Day;
                    }
                    break;
            }

            return false;
        }


        public string SetAttachedItems(List<ItemReward> attachments)
        {
            string result = "";
            foreach (ItemReward itemReward in attachments)
            {
                result += itemReward.Id + ":" + itemReward.Quantity + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        public void SetItemAndQuantity(string itemId, int quantity)
        {
            // find if the item is already in the attachments
            if (Attachments.Contains(itemId.ToString()))
            {
                // if the item is already in the attachments, update the quantity
                string[] items = Attachments.Split(',');
                for (int i = 0; i < items.Length; i++)
                {
                    string[] keyValue = items[i].Split(':');
                    if (keyValue[0] == itemId.ToString())
                    {
                        items[i] = itemId + ":" + quantity;
                        Attachments = string.Join(",", items);
                        return;
                    }
                }
            }
            else
            {
                // if the item is not in the attachments, add the item
                Attachments += "," + itemId + ":" + quantity;
            }
        }

        public SystemMail() : base() { }
        public SystemMail(string documentName) : base(documentName) { }
    }
}
