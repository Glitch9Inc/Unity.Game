using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9
{
    /// <summary>
    /// Utility class that makes life easier when playing sounds.
    /// </summary>
    public static class Sounds
    {
        public static void Play(UISoundType uiSound)
        {
            if (SoundManager.Instance.LogIfNull()) return;
            SoundManager.Instance.PlaySFX(UISounds.GetAddressableName(uiSound));
        }

        public static void PlayWithAddressableName(string addressableName)
        {
            if (SoundManager.Instance.LogIfNull()) return;
            SoundManager.Instance.PlaySFX(addressableName);
        }

        public static void PlayWithLocalPath(SoundType soundType, string persistentDatapathPath)
        {
            if (SoundManager.Instance.LogIfNull()) return;
            AudioType audioType = AudioUtils.GetAudioTypeFromFilePath(persistentDatapathPath);
            if (audioType == AudioType.UNKNOWN) return;
            CoroutineOwner.StartCoroutine(SoundManager.Instance.PlayFileInPersistentDatapath(soundType, persistentDatapathPath, audioType));
        }
    }
}