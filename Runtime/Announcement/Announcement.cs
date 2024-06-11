using System;
using UnityEngine;

namespace Glitch9.Game
{
    [Serializable]
    public class Announcement
    {
        public AnnounceType type;
        public Sprite sprite;
        public string audioAsset;
        public string message;
        public bool isGlobal;
    }
}