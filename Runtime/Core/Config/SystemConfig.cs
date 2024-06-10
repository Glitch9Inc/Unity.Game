using Glitch9.Apis.Google.Firebase;

using UnityEngine;

namespace Glitch9
{
    public static class SystemConfig
    {
        private const string PREF_LANGUAGE = "Language";
        private const string TERMS_OF_SERVICE_AGREED = "TermsOfServiceAgreed";
        private const string AUTH_TYPE = "AuthType";
        private const string MASTER_VOLUME = "MasterVolume";
        private const string SFX_VOLUME = "SFXVolume";
        private const string VOICE_VOLUME = "VoiceVolume";
        private const string BGM_VOLUME = "BGMVolume";
        private const string BGM_BACKGROUND_PLAY = "BGMBackgroundPlay";
        private const string BGM_BACKGROUND_CONTROLS = "BGMBackgroundControls";

        public static Locale Locale
        {
            get => (Locale)PlayerPrefs.GetInt(PREF_LANGUAGE, (int)SystemLanguage.English);
            set
            {
                PlayerPrefs.SetInt(PREF_LANGUAGE, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static FirebaseAuthType AuthType
        {
            get => (FirebaseAuthType)PlayerPrefs.GetInt(AUTH_TYPE, (int)FirebaseAuthType.None);
            set
            {
                PlayerPrefs.SetInt(AUTH_TYPE, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static float MasterVolume
        {
            get => PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
            set
            {
                PlayerPrefs.SetFloat(MASTER_VOLUME, value);
                PlayerPrefs.Save();
            }
        }

        public static float SfxVolume
        {
            get => PlayerPrefs.GetFloat(SFX_VOLUME, 1f);
            set
            {
                PlayerPrefs.SetFloat(SFX_VOLUME, value);
                PlayerPrefs.Save();
            }
        }

        public static float VoiceVolume
        {
            get => PlayerPrefs.GetFloat(VOICE_VOLUME, 1f);
            set
            {
                PlayerPrefs.SetFloat(VOICE_VOLUME, value);
                PlayerPrefs.Save();
            }
        }

        public static float BgmVolume
        {
            get => PlayerPrefs.GetFloat(BGM_VOLUME, 1f);

            set
            {
                PlayerPrefs.SetFloat(BGM_VOLUME, value);
                PlayerPrefs.Save();
            }
        }

        public static UnixTime TermsOfServiceAgreed
        {
            get
            {
                if (!PlayerPrefs.HasKey(TERMS_OF_SERVICE_AGREED))
                {
                    PlayerPrefs.SetInt(TERMS_OF_SERVICE_AGREED, (int)UnixTime.MinValue);
                    PlayerPrefs.Save();
                }
                // check if the value is valid
                long unixTime = PlayerPrefs.GetInt(TERMS_OF_SERVICE_AGREED);
                if (unixTime < UnixTime.MinValue || unixTime > UnixTime.MaxValue)
                {
                    PlayerPrefs.SetInt(TERMS_OF_SERVICE_AGREED, (int)UnixTime.MinValue);
                    PlayerPrefs.Save();
                }

                return unixTime;
            }
            set
            {
                PlayerPrefs.SetInt(TERMS_OF_SERVICE_AGREED, (int)value);
                PlayerPrefs.Save();
            }
        }
        public static bool BgmBackgroundPlay
        {
            get => PlayerPrefs.GetInt(BGM_BACKGROUND_PLAY, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(BGM_BACKGROUND_PLAY, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public static bool BgmBackgroundControls
        {
            get => PlayerPrefs.GetInt(BGM_BACKGROUND_CONTROLS, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(BGM_BACKGROUND_CONTROLS, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
    }
}
