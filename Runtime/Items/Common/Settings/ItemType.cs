using System;
using UnityEngine;

namespace Glitch9.Game
{
    [Serializable]
    public class ItemType
    {
        [Tooltip("아이템 클래스의 타입 이름")]
        public string @class;

        [Tooltip("필터등의 UI에서 사용할 아이콘")]
        public Sprite icon;

        [Tooltip("이 타입의 아이템을 인벤토리에 표시하는지 여부")]
        public bool hideFromInventory;
    }
}
