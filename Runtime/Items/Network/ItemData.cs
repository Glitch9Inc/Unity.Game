using Glitch9.Apis.Google.Firestore;
using Glitch9.Cloud;
using Glitch9.DB;
using UnityEngine;

namespace Glitch9.Game
{
    /// <summary>
    /// 아이템의 세이브 데이터를 저장하는 클래스입니다.
    /// </summary>
    public class ItemData : Firedata<ItemData>, IModel
    {
        public string Key => Id;
        public Item GetItem() => ItemDatabase.Get(Id);
        public Sprite Icon => GetItem()?.Icon;

        /// <summary>
        ///  아이템의 Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 아이템의 수량
        /// </summary>
        [CloudData] public int Quantity { get; set; }

        /// <summary>
        /// 아이템을 획득한 날짜
        /// </summary>
        [CloudData] public UnixTime AcquiredAt { get; set; }

        public ItemData() { }

        /// <summary>
        /// Firestore에서 가져온 데이터를 적용하는 생성자
        /// </summary>
        public ItemData(string id)
        {
            Id = id;
        }

        public ItemData(int itemIndex)
        {
            Id = ItemDatabase.Get(itemIndex).Id;
        }

        /// <summary>
        /// 신규 획득한 아이템을 생성하는 생성자
        /// </summary>
        public ItemData(string id, int quantity)
        {
            Id = id;
            Quantity = quantity;
            AcquiredAt = UnixTime.Now;
        }
    }

}