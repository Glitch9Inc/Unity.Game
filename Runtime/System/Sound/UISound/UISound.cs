using System;
using UnityEngine;

namespace Glitch9
{
    [Serializable]
    public class UISound
    {
        public UISoundType type;
        public AudioClip clip;
        public float volume;
        public string addressableName;
    }
}