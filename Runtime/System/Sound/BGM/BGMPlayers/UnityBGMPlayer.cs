using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Glitch9.Game
{
    public class UnityBGMPlayer : IBGMPlayer
    {
        public int CurrentMediaIndex => _currentTrack;
        private List<ISoundTrack> _playlist;
        private int _currentTrack;
        private bool _isPreviewing = false;
        private bool _isInitialized = false;
        private string _currentUri;
        private CancellationTokenSource _cts;

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
        }

        public void Prepare(List<ISoundTrack> bgmList)
        {
            Initialize();
            _playlist = bgmList;
            _currentTrack = 0;
            Play();
        }

        public void Play()
        {
            Play(_currentTrack);
        }

        public void Play(int index)
        {
            try
            {
                if (_playlist.LogIfNullOrEmpty())
                {
                    GNLog.Error($"Play: {typeof(UnityBGMPlayer).Name}의 플레이리스트가 비어있습니다.");
                    return;
                }

                if (index < 0 || index >= _playlist.Count)
                {
                    GNLog.Error($"Play: {index}는 플레이리스트의 범위를 벗어났습니다.");
                    return;
                }

                ISoundTrack currentMusic = _playlist[index];
                if (currentMusic.LogIfNull())
                {
                    GNLog.Error($"Play: {index}번째 BGM이 비어있습니다.");
                    return;
                }

                PlayWithUri(currentMusic.Uri, false);
            }
            catch (Exception e)
            {
                GNLog.Exception(e);
            }
        }

        public void Next()
        {
            _currentTrack++;
            if (_currentTrack >= _playlist.Count)
            {
                _currentTrack = 0;
            }

            Play(_currentTrack);
        }

        public void Previous()
        {
            _currentTrack--;
            if (_currentTrack < 0)
            {
                _currentTrack = _playlist.Count - 1;
            }

            Play(_currentTrack);
        }

        public void Release()
        {
            SoundManager.Instance.StopBGM();
            _playlist = null;
        }

        public void SetVolume(float volume)
        {
            SoundManager.Instance.SetBGMVolume(volume);
        }

        public void FadeOut(float duration = 3)
        {
            SoundManager.Instance.FadeOutBGM(duration);
        }

        public void StartPreview(int trackNumber)
        {
            StartPreview(_playlist[trackNumber]);
        }
        
        public void StartPreview(ISoundTrack music)
        {
            if (music.LogIfNull()) return;
            PlayWithUri(music.Uri, true);
        }

        public void StopPreview()
        {
            if (_isPreviewing)
            {
                Play(_currentTrack);
                _isPreviewing = false;
            }
        }

        public async void PlayWithUri(string uri, bool preview)
        {
            if (string.IsNullOrEmpty(uri))
            {
                GNLog.Error($"PlayWithUri: uri가 비어있습니다.");
                return;
            }

            if (_cts != null && !_cts.Token.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();

            _currentUri = uri;
            _isPreviewing = preview;

            try
            {
                AudioSource audioSource = await SoundManager.Instance.PlayBGMAsync(uri, () => BGMManager.Instance.UpdateMusicInformation(GetCurrentTrack()), _cts.Token);
                if (audioSource == null || _cts.Token.IsCancellationRequested) return;
                await WaitForAudioEndAsync(audioSource, _cts.Token);
            }
            catch
            {
                // 여기서 일어나는 에러는 별거 아니므로 워닝
                GNLog.Warning("PlayWithUri 작업이 취소되었습니다.");
            }
        }

        private async UniTask WaitForAudioEndAsync(AudioSource audioSource, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            float duration = audioSource.clip.length;
            int durationInMilliseconds = (int)(duration * 1000);
            await UniTask.Delay(durationInMilliseconds, cancellationToken: token);
            if (token.IsCancellationRequested) return;
            if (!_isPreviewing) Next();
            else PlayWithUri(_currentUri, true);
        }

        public ISoundTrack GetCurrentTrack()
        {
            if (!_isInitialized)
            {
                GNLog.Error($"GetCurrentTrack(): {typeof(UnityBGMPlayer).Name}가 초기화되지 않았습니다.");
                return null;
            }
            if (_playlist.LogIfNullOrEmpty()) return null;
            return _playlist[_currentTrack];
        }
    }
}
