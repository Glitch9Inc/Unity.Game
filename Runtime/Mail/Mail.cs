using System;
using System.Collections.Generic;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Apis.Google.Firestore.Tasks;
using Glitch9.Cloud;

using Glitch9.Routina;

using Glitch9.Toolkits.SmartLocalization;
using Glitch9.UI;

namespace Glitch9.Game.MailSystem
{
    public class Mail : FirestoreDocument<Mail>, IModel
    {
        public override string Key => Date.ToString();

        /// <summary>
        /// 메일 고유번호 (시스템 메일)
        /// </summary>
        [CloudData] public int Id { get; set; } = -1;

        /// <summary>
        /// 메일 발신자
        /// </summary>
        [CloudData] public string Sender { get; set; } = "System";

        /// <summary>
        /// 메일 제목
        /// </summary>
        [CloudData] public string Subject { get; set; } = "";

        /// <summary>
        /// 메일 내용
        /// </summary>
        [CloudData] public string Content { get; set; } = "";

        /// <summary>
        /// 메일 발송일 
        /// </summary>
        [CloudData] public UnixTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// 메일의 종류 (로그인 보상, 이벤트 보상, 점검 보상, 시스템) 
        /// </summary>
        [CloudData] public SystemMailCondition Type { get; set; } = SystemMailCondition.None;

        /// <summary>
        /// 메일의 상태 (읽음, 안읽음, 아이템받음)
        /// </summary>
        [CloudData] public MailStatus State { get; set; } = MailStatus.None;

        /// <summary>
        /// 메일에 첨부된 게임 아이템 (아이템Id, 수량)
        /// </summary>
        [CloudData] public List<ItemReward> Attachments { get; set; } = new();


        public async void Send(Action<IResult> onComplete = null, string email = null)
        {
            Date = DateTime.Now;
            int batchId = 381501;

            if (Id != -1)
            {
                User.ClientData.ReceivedSystemMails.Add(Id);
                User.ClientData.SetMergeBatch(batchId);
            }

            this.SetMergeBatch(batchId);
            var result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
            if (result.IsSuccess) UIManager.Instance.MakeAnnouncement(AnnounceType.NewMail, "new_mail_received".Localize());
        }

        public void Add(string itemId, int quantity)
        {
            Attachments.Add(ItemReward.FromItemId(itemId, quantity));
        }
    }
}