using System.Collections.Generic;
using UnityEngine;

namespace Glitch9
{
    public class UISounds : ScriptableResource<UISounds>
    {
        [SerializeField] private List<UISound> uiSounds = new();

        public static string GetAddressableName(UISoundType uiSound)
        {
            return Instance.uiSounds.Find(x => x.type == uiSound)?.addressableName;
        }
    }
}