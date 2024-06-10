using Cysharp.Threading.Tasks;
using Glitch9.NativePlugins.NativeMediaPlayer;
using System;
using System.Collections.Generic;

namespace Glitch9.Game
{
    public class MusicManager : MonoSingleton<MusicManager>
    {
        public Action<IMusic> OnMusicChanged;
        public bool IsInitialized;

        /// <summary>
        /// 미디어 플레이어(플러그인)을 사용하는지 여부
        /// false면 유니티 오디오 소스를 사용하는것
        /// </summary>
        public bool IsUsingNmp;
        private IAudioPlayer _player;

        public void Initialize(string startUri, Action<IMusic> onMusicChanged, bool useNMP)
        {
            IsUsingNmp = useNMP;
            OnMusicChanged = onMusicChanged;
            _player = useNMP ? new NativeAudioPlayer() : new UnityAudioPlayer();
            _player.Initialize();
            _player.SetVolume(SystemConfig.BgmVolume);
            _player.PlayWithUri(startUri, true);
            IsInitialized = true;
        }

        public async void SetPlaylist(List<IMusic> playlist)
        {
            FadeOut(3f);                    // 오프닝에서 쓰던 BGM이 서서히 FadeOut된다.
            await UniTask.Delay(3000);      // FadeOut이 끝날때까지 기다린다.
            GNLog.Info($"사용자 플레이리스트가 초기화되었습니다: <color=blue>{playlist.Count}</color>개의 트랙");
            _player.Prepare(playlist);      // BGM을 준비한다.            
            IsInitialized = true;
        }

        public void Next() => _player.Next();
        public void Previous() => _player.Previous();
        public void StartPreview(IMusic music) => _player.StartPreview(music);
        public void StopPreview() => _player.StopPreview();
        public void FadeOut(float duration = 3f) => _player.FadeOut(duration);
        public void Play(int index) => _player.Play(index);
        public void UpdateMusicInformation(IMusic current) => OnMusicChanged?.Invoke(current);

        /// <summary>
        /// BGM을 백그라운드에서 재생할지 여부를 설정한다. (플러그인 사용시에만 가능)
        /// </summary>
        public void SetBackgroundMode(bool value)
        {
            if (IsUsingNmp)
            {
                MediaPlayer.SetBackgroundMode(value);
                SystemConfig.BgmBackgroundPlay = value;
                return;
            }

            GNLog.Warning("SetBackgroundMode is not supported in Unity Audio Source.");
        }

        /// <summary>
        /// 백그라운드에서 재생중에 컨트롤러를 표시할지 여부를 설정한다. (플러그인 사용시에만 가능)
        /// Android : MediaStyleNotification
        /// iOS     : MPNowPlayingInfoCenter
        /// </summary>
        public void SetBackgroundControls(bool value)
        {
            if (IsUsingNmp)
            {
                MediaPlayer.SetBackgroundControls(value);
                SystemConfig.BgmBackgroundControls = value;
                return;
            }

            GNLog.Warning("SetBackgroundControls is not supported in Unity Audio Source.");
        }
    }
}