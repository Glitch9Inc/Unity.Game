using Cysharp.Threading.Tasks;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Apis.Google.Firestore.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Glitch9.Game
{
    /// <summary>
    /// 미션 보상을 나타내는 클래스입니다.
    /// (구글시트에 정의된 미션 보상을 가져오기 위한 클래스이기도 합니다.)
    /// </summary>
    public class RewardData
    {
        /*         
            2024-02-01: Munchkin 구글시트에서 원래 GNItemSave를 쓰고있었지만, 
            클래스의 한계로 인해 MissionReward로 변경하였습니다.
        */
        public int ExpReward { get; set; } = 0;
        public int SeasonExpReward { get; set; } = 0;
        public List<IReward> Rewards { get; private set; } = new();

        public void Add(IReward reward)
        {
            Rewards.Add(reward);
        }

        /// <summary>
        /// 이게 있어야 구글시트에서 가져온 데이터를 사용할 수 있습니다. (private, protected 금지)
        /// </summary>
        public RewardData() { }

        /// <summary>
        /// 구글시트 Cell Format: 
        /// Exp, SeasonExp만 있을때: [ Exp, SeasonExp ] (대괄호 포함)
        /// Exp, SeasonExp, IReward(1개) 있을때: [ Exp, SeasonExp, IReward ] (대괄호 포함)
        /// 
        /// IReward 형식:
        /// - ItemStock: (string)ItemId:(int)Quantity
        /// - AlarmSave: voice(고정):(int)voiceIndex:(string)Companion
        ///
        /// 예시:
        /// - [ 100, 100, credit:100, crystal:50, voice:201:tuto, voice:302:tuto ]
        /// 
        /// [주의!] Exp와 SeasonExp는 무조건 있어야합니다. (없으면 생성자에서 처리하지 않습니다.)
        /// </summary>
        /// <param name="googleSheetCell"></param>
        public RewardData(string googleSheetCell)
        {
            if (googleSheetCell.LogIfNullOrEmpty()) return;

            // 셀 값의 앞뒤 대괄호 제거
            googleSheetCell = googleSheetCell.Trim(new char[] { '[', ']' });
            string[] split = googleSheetCell.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // 최소 Exp와 SeasonExp는 있어야 함
            if (split.Length < 2) return;

            // Exp와 SeasonExp 처리
            if (int.TryParse(split[0], out int exp)) ExpReward = exp;
            if (int.TryParse(split[1], out int seasonExp)) SeasonExpReward = seasonExp;

            // 나머지 항목 처리
            for (int i = 2; i < split.Length; i++)
            {
                IReward reward = RewardUtils.CreateReward(split[i]);
                if (reward == null) continue;
                Rewards.Add(reward);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"[ {ExpReward}, {SeasonExpReward}");
            foreach (IReward reward in Rewards)
            {
                if (reward == null) continue;
                sb.Append($", {reward.Id}:{reward.Quantity}");
            }
            sb.Append(" ]");
            return sb.ToString();
        }

        public async UniTask TryClaimAsync(Action<IResult> onComplete = null, bool showClaimView = true)
        {
            int batchId = DateTimeId.CreateNew();
            SetClaimBatch(batchId, showClaimView);

            onComplete += OnRewardClaimed;
            IResult result = await Firetask.ExecuteBatchAsync(batchId);
            onComplete?.Invoke(result);
        }

        private void OnRewardClaimed(IResult result)
        {
            MyGame.HandleResult(this, result);
            
            if (result.IsFailure)
            {
                if (ExpReward > 0)
                {
                    MyGame.User.Experience -= ExpReward;
                }

                if (SeasonExpReward > 0)
                {
                    //SeasonPassSaveData seasonData = SeasonPassManager.Instance.GetCurrenetPlayerData();
                    //seasonData?.UndoExpGain(SeasonExpReward);
                }
            }
        }

        public int SetClaimBatch(int batchId = -1, bool showClaimView = true)
        {
            if (showClaimView)
            {
                //TODO: 보상 받을 때 연출 추가
                //onSuccess += (success) => saves.ShowRewardResult();
            }

            if (batchId == -1) batchId = DateTimeId.CreateNew();

            if (ExpReward > 0)
            {
                MyGame.User.Experience += ExpReward;
            }

            if (SeasonExpReward > 0)
            {
                //SeasonPassSaveData seasonData = SeasonPassManager.Instance.GetCurrenetPlayerData();
                //if (seasonData.LogIfNull())
                //{
                //    UIManager.Instance.DisplayError(Issue.UnknownError);
                //    return -1;
                //}

                //seasonData.SetExpGainBatch(batchId, SeasonExpReward);
            }

            if (Rewards.IsValid())
            {
                bool isVoiceUpdated = false;

                foreach (IReward reward in Rewards)
                {
                    if (reward == null) continue;
                    if (reward is ItemReward itemReward)
                    {
                        Inventory.SetAcquireBatch(itemReward.Id, itemReward.Quantity, batchId);
                    }
                    //else if (reward is VoiceAlarmSave voice)
                    //{
                    //    string firestoreId = VoiceAlarmSave.CreateId(voice.CompanionId, voice.Title);
                    //    User.VoiceAlarms.AddOrUpdate(firestoreId, voice);
                    //    isVoiceUpdated = true;
                    //}
                }

                //if (isVoiceUpdated)
                //{
                //    User.VoiceAlarms.SetMergeBatch(batchId);
                //}
            }

            return batchId;
        }
    }
}