using System;

namespace Glitch9
{
    public class FriendUtility
    {
        /// <summary>
        /// 친구 요청을 보낸다.
        /// </summary>
        public void SendFriendRequest(string targetEmail, Action<bool> onResult, string email = null)
        {
            //Dictionary<DocumentReference, Dictionary<string, object>> batch = new Dictionary<DocumentReference, Dictionary<string, object>>();

            //// TODO 재작업 필요

            ////DocumentReference myRef = GetLocation().GetDocument(email: email);
            ////batch.Add(myRef, (targetEmail, (int)FriendStatus.Accepted).ToFirestoreField());

            ////DocumentReference friendRef = GetLocation().GetDocument(email: targetEmail);
            ////batch.Add(friendRef, (email, (int)FriendStatus.Requested).ToFirestoreField());

            //batch.Batch(onResult);
        }

        /// <summary>
        /// 친구 요청을 수락한다.
        /// </summary>
        public void AcceptFriendRequest(string targetEmail, Action<bool> onError = null, string email = null)
        {
            // TODO 재작업 필요

            //if (string.IsNullOrEmpty(targetEmail))
            //{
            //    UIManager.Instance.Error("Invalid Friend Id");
            //    return;
            //}

            //DocumentReference docRef = GetLocation().GetDocument(email: email);
            //docRef.MergeDocument((targetEmail, (int)FriendStatus.Accepted).ToFirestoreField(), onError);
        }

        /// <summary>
        /// 친구 상태를 불러온다.
        /// </summary>
        public void GetTargetFriendStatus(string targetEmail, Action<FriendStatus> onLoaded, Action<bool> onResult = null)
        {
            // DocumentReference targetRef = GetDocument(targetEmail); //GNFirestore.DataCollection(targetEmail).Document("Friends");

            // targetRef.GetSnapshot((dict) =>
            // {
            //     var result = dict.ToDictionary();

            //     // GNFriends friends = (GNFriends)SetDictionary(dict);
            //     // string myEmail = GNUser.MyInstance.Email;

            //     // if (!friends.IsNull())
            //     // {
            //     //     if (friends.Dict.Count > 0 && friends.Dict.ContainsKey(myEmail))
            //     //     {
            //     //         int state = Convert.ToInt32(friends.Dict[myEmail].Status);
            //     //         FriendStatus friendState = (FriendStatus)state;
            //     //         onLoaded?.Invoke(friendState);
            //     //     }
            //     //     else
            //     //     {
            //     //         onLoaded?.Invoke(FriendStatus.None);
            //     //     }
            //     // }

            // }, onResult);
        }

        /// <summary>
        /// 친구를 삭제한다. 친구 요청을 거절할때도 사용한다.
        /// </summary>
        public void RemoveFriend(string targetEmail, Action<bool> onResult)
        {
            //Dictionary<DocumentReference, object> batch = new Dictionary<DocumentReference, object>();

            //DocumentReference targetRef = Server.GetDataCollection(targetEmail).Document("Friends");
            //DocumentReference myRef = GetDocument();

            //Dictionary<string, object> targetUpdate = new Dictionary<string, object>
            //{
            //    { User.Email, FieldValue.Delete }
            //};

            //Dictionary<string, object> myUpdate = new Dictionary<string, object>
            //{
            //    { targetEmail, FieldValue.Delete }
            //};

            //batch.Add(targetRef, targetUpdate);
            //batch.Add(myRef, myUpdate);

            //batch.Batch(onResult);
        }
    }
}
