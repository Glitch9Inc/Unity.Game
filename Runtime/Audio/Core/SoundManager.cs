using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using Glitch9.IO.Network;
using Glitch9.IO;
using Glitch9.IO.Files;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.Networking;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Glitch9
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private const string DEFAULT_EXT = "mp3";
        private const string MIXER_MASTER = "MasterVolume";
        private const string MIXER_BGM = "BgmVolume";
        private const string MIXER_SFX = "SfxVolume";
        private const string MIXER_VOICE = "VoiceVolume";


#if ODIN_INSPECTOR
        [ShowInInspector, PropertyRange(0f, 1f)]
#endif
        public float MasterVolume
        {
            get => SystemConfig.MasterVolume;
            set => SetMasterVolume(value);
        }

#if ODIN_INSPECTOR
        [ShowInInspector, PropertyRange(0f, 1f)]
#endif
        public float BgmVolume
        {
            get => SystemConfig.BgmVolume;
            set => SetBGMVolume(value);
        }

#if ODIN_INSPECTOR
        [ShowInInspector, PropertyRange(0f, 1f)]
#endif
        public float SfxVolume
        {
            get => SystemConfig.SfxVolume;
            set => SetSfxVolume(value);
        }

#if ODIN_INSPECTOR
        [ShowInInspector, PropertyRange(0f, 1f)]
#endif 
        public float VoiceVolume
        {
            get => SystemConfig.VoiceVolume;
            set => SetVoiceVolume(value);
        }

        private AudioMixer AudioMixer => _sfxAudioSource.outputAudioMixerGroup.audioMixer;

        private AudioSource _sfxAudioSource;
        private AudioSource _voiceAudioSource;
        private AudioSource _bgmAudioSource;


        protected override void Awake()
        {
            base.Awake();
            _sfxAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
            _voiceAudioSource = transform.GetChild(1).GetComponent<AudioSource>();
            _bgmAudioSource = transform.GetChild(2).GetComponent<AudioSource>();
        }

        private void Start()
        {
            SetMasterVolume(SystemConfig.MasterVolume);
            SetBGMVolume(SystemConfig.BgmVolume);
            SetSfxVolume(SystemConfig.SfxVolume);
            SetVoiceVolume(SystemConfig.VoiceVolume);
        }

        public void SetMasterVolume(float value)
        {
            SetVolume(MIXER_MASTER, value);
            SystemConfig.MasterVolume = value;
        }

        public void SetSfxVolume(float value)
        {
            SetVolume(MIXER_SFX, value);
            SystemConfig.SfxVolume = value;
        }

        public void SetVoiceVolume(float value)
        {
            SetVolume(MIXER_VOICE, value);
            SystemConfig.VoiceVolume = value;
        }

        public void SetBGMVolume(float value)
        {
            SetVolume(MIXER_BGM, value);
            SystemConfig.BgmVolume = value;
        }

        private void SetVolume(string mixerName, float value)
        {
            float log = RetrieveLog10(value);
            if (log < -40f) log = 0f;
            AudioMixer.SetFloat(mixerName, log);
        }

        /// <summary>
        /// 존나 긴 로그를 출력할때 쓰는 함수
        /// </summary>
        private float RetrieveLog10(float value) => Mathf.Log10(value) * 20;
        public void PlaySFX(string key) => PlayAudioClip(SoundType.SFX, key);
        public void PlaySFX(AudioClip clip) => PlayAudioClip(SoundType.SFX, clip);
        public void StopSfx() => _sfxAudioSource.Stop();
        public void PlayVoice(string key) => PlayAudioClip(SoundType.Voice, key);
        public void PlayVoice(AudioClip clip) => PlayAudioClip(SoundType.Voice, clip);
        public void StopVoice() => _voiceAudioSource.Stop();

        /// <summary>
        /// Firebase Cloud Storage에서 BGM을 다운받아 재생합니다.
        /// 작업이 끝나면 오디오소스(Player) 인스턴스를 반환합니다.
        /// </summary>
        public async UniTask<AudioSource> PlayBgmAsync(string fileUri, Action onPrepared, CancellationToken token)
        {
            await DownloadAndPlayAsync(_bgmAudioSource, fileUri, GetLocalBgmPath(fileUri), token, onPrepared);
            return _bgmAudioSource;
        }

        public void StopBGM() => _bgmAudioSource.Stop();
        public void FadeOutBGM(float duration = 3f) => FadeOutPlayer(_bgmAudioSource, duration);

        private string GetLocalBgmPath(string fileUri)
        {
            string filename = Path.GetFileName(fileUri);
            return System.IO.Path.Combine(Application.persistentDataPath + "/BGM/", filename);
        }

        private AudioSource GetPlayer(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.SFX => _sfxAudioSource,
                SoundType.Voice => _voiceAudioSource,
                SoundType.BGM => _bgmAudioSource,
                _ => null,
            };
        }

        private void PlayAudioClip(SoundType type, string key)
        {
            if (string.IsNullOrEmpty(key) || !Application.isPlaying) return;

            try
            {
                Addressables.LoadAssetAsync<AudioClip>(key).Completed += (clip) =>
                {
                    AudioClip audioClip = clip.Result;
                    if (audioClip != null)
                    {
                        PlayAudioClip(type, audioClip);
                    }
                    else
                    {
                        GNLog.Error("audio_missing : " + key);
                    }
                };
            }
            catch
            {
                GNLog.Warning("audio_missing : " + key);
            }
        }

        private void PlayAudioClip(SoundType type, AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource player = GetPlayer(type);
                if (player.isPlaying)
                {
                    if (type == SoundType.SFX)
                    {
                        PlayWithTempAudioSource(clip);
                        return;
                    }
                    else if (type == SoundType.BGM)
                    {
                        FadeOutPlayer(player, 3f, () =>
                        {
                            player.clip = clip;
                            player.Play();
                        });
                        return;
                    }
                    else if (type == SoundType.Voice)
                    {
                        player.Stop();
                    }
                }
                player.clip = clip;
                player.Play();
            }
        }

        private void FadeOutPlayer(AudioSource player, float duration, Action onComplete = null)
        {
            StartCoroutine(FadeOutPlayerCoroutine(player, duration, onComplete));
        }

        private IEnumerator FadeOutPlayerCoroutine(AudioSource player, float duration, Action onComplete = null)
        {
            float startVolume = player.volume;

            while (player.volume > 0)
            {
                player.volume -= startVolume * Time.deltaTime / duration;
                yield return null;
            }

            player.Stop();
            player.volume = startVolume;
            onComplete?.Invoke();
        }

        private void PlayWithTempAudioSource(AudioClip audioClip)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = _sfxAudioSource.outputAudioMixerGroup;
            newAudioSource.clip = audioClip;

            // Play the sound
            newAudioSource.Play();

            // Once the sound has finished playing, remove the AudioSource
            Destroy(newAudioSource, audioClip.length);
        }

        public async UniTask DownloadAndPlayAsync(AudioSource audioSource, string fileUri, string localPath, CancellationToken token, Action onPrepared = null)
        {
            if (string.IsNullOrEmpty(fileUri))
            {
                GNLog.Error("fileUri is null or empty");
                return;
            }

            GNLog.Info($"DownloadAndPlayAsync: <color=blue>{fileUri}</color>");
            string extension = Path.GetExtension(fileUri);

            if (extension == string.Empty)
            {
                string defaultExtension = $".{DEFAULT_EXT}"; // 만약 확장자가 없다면 .mp3로 강제 지정
                fileUri += defaultExtension;
                extension = defaultExtension;
            }
            else
            {
                string doubleExtension = extension + extension;
                if (fileUri.Contains(doubleExtension)) fileUri = fileUri.Replace(doubleExtension, extension);
            }

            AudioType audioType = AudioUtils.GetAudioTypeFromFileExtension(extension);

            if (!File.Exists(localPath))
            {
                await UnityDownloader.DownloadFileAsync(fileUri, localPath);
            }

            /* 파일 사이즈 체크 */
            FileInfo fileInfo = new(localPath);
            long size = fileInfo.Length;

            if (size < 1024) // 5kb
            {
                await UnityDownloader.DownloadFileAsync(fileUri, localPath);
            }

            if (token.IsCancellationRequested) return;

            await AwaitCoroutine(PlayFileInPersistentDatapath(audioSource, localPath, audioType, token, onPrepared));
        }

        private UniTask AwaitCoroutine(IEnumerator coroutine)
        {
            UniTaskCompletionSource<bool> completionSource = new();
            StartCoroutine(RunCoroutine(coroutine, completionSource));
            return completionSource.Task;
        }

        private IEnumerator RunCoroutine(IEnumerator coroutine, UniTaskCompletionSource<bool> completionSource)
        {
            yield return StartCoroutine(coroutine);     // Run the coroutine
            completionSource.TrySetResult(true);        // Signal completion
        }

        public IEnumerator PlayFileInPersistentDatapath(SoundType soundType, string localPath, AudioType audioType, Action onPrepared = null)
        {
            AudioSource audioSource = GetPlayer(soundType);
            yield return PlayFileInPersistentDatapath(audioSource, localPath, audioType, default, onPrepared);
        }

        public IEnumerator PlayFileInPersistentDatapath(AudioSource player, string localPath, AudioType audioType, CancellationToken token = default, Action onPrepared = null)
        {
            if (string.IsNullOrEmpty(localPath)) // check if localPath is valid
            {
                GNLog.Error("localPath is null or empty");
                yield break;
            }

            if (!localPath.StartsWith("file://")) // if localPath doesn't start with "file://", add it
            {
                localPath = "file://" + localPath;
            }

            // log localPath and audioType
            GNLog.Info($"<color=blue>PlayFileInPersistentDatapath: {localPath}/{audioType}</color>");

            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(localPath, audioType); // AudioType.MPEG은 임시
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                GNLog.Error($"PlayFileInPersistentDatapath Failed! Path: {localPath}, Reason: {www.error}");
            }
            else
            {
                if (token != default && token.IsCancellationRequested) yield break;

                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                player.time = 0;
                player.clip = audioClip;
                player.Play();
                onPrepared?.Invoke();
            }
        }
    }
}