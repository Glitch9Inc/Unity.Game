using Glitch9.Apis.Google.Firestore;
using Glitch9.Game.MailSystem;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public class MailManager : MonoSingleton<MailManager>
    {
        public FirestoreDictionary<SystemMail> SystemMails { get; set; }

        private void Start()
        {
            MyGame.OnGameStart += (sender, args) =>
            {
                if (SystemMails == null)
                {
                    GNLog.Error("자동발신 시스템메일이 Null입니다.");
                    return;
                }

                if (SystemMails.Count == 0)
                {
                    GNLog.Error("자동발신 시스템메일이 하나도 없습니다. 서버를 확인해주세요.");
                    return;
                }

                StartCoroutine(CheckSystemMails(() => LoadMail()));
            };
        }

        public void LoadMail(Action<bool> onResult = null)
        {
            MyGame.OnMailReceived += (sender, mail) =>
            {
                //MenuManager.Instance.SetNewInformation("Inbox", HasUnreadMail());
                //User.Mails.OrderByDescending(entry => entry.Key).ToDictionary(entry => entry.Key, entry => entry.Value);
            };
        }

        public void ResetMailAvailable()
        {
            if (HasUnreadMail()) MyGame.OnMailReceived?.Invoke(this, null);
        }

        private bool HasUnreadMail()
        {
            if (GameManager.Instance.User.Mails.IsNullOrEmpty()) return false;

            foreach (KeyValuePair<string, Mail> mail in GameManager.Instance.User.Mails)
            {
                if (mail.Value.Status == MailStatus.None) return true;
            }

            return false;
        }

        private IEnumerator CheckSystemMails(Action onComplete)
        {
            foreach (KeyValuePair<string, SystemMail> systemMail in SystemMails)
            {
                if (GameManager.Instance.User.ReceivedSystemMails != null && GameManager.Instance.User.ReceivedSystemMails.Contains(systemMail.Value.Index)) continue;
                // If the mail has not been sent yet, check the conditions and send it.
                GameManager.Instance.User.ReceivedSystemMails ??= new List<int>();

                bool conditionMatches = systemMail.Value.IsAvailable();

                if (conditionMatches)
                {
                    Mail mail = systemMail.Value.ToMail();
                    mail.Send();
                    GNLog.Info("시스템메일 발송 : " + mail.Title);
                    // Add the sent mail to the User's received mails
                    GameManager.Instance.User.ReceivedSystemMails.Add(systemMail.Value.Index);
                }

                yield return null;
            }

            onComplete?.Invoke();
        }

    }
}