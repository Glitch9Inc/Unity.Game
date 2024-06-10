using Cysharp.Threading.Tasks;
using Glitch9.NativePlugins.NativeMediaPlayer;
using System.Collections.Generic;


namespace Glitch9.Game
{
    public class NativeSoundPlayer : ISoundPlayer
    {
        public int CurrentMediaIndex => _currentTrack;
        private List<IMusic> _playlist;
        private Playlist _nmpPlaylist;
        private int _currentTrack;
        private int _bgmId;

        public void Initialize()
        {
            MediaEvents.onInitialized += (initialized) =>
            {
                if (initialized)
                {
                    MusicManager.Instance.SetBackgroundControls(SystemConfig.BgmBackgroundControls);
                    MusicManager.Instance.SetBackgroundMode(SystemConfig.BgmBackgroundPlay);
                    MediaPlayer.Volume = SystemConfig.BgmVolume;
                }
            };

            MediaEvents.onPrepared += () =>
            {
                IMusic music = _playlist[CurrentMediaIndex];
                MusicManager.Instance.UpdateMusicInformation(music);
            };

            MediaPlayer.RepeatMode = RepeatMode.RepeatAll;
            MediaPlayer.Play();
        }

        public void Prepare(List<IMusic> bgmList)
        {
            _playlist = bgmList;
            Playlist playlist = ParsePlaylist(bgmList);
            MediaPlayer.SetPlaylist(playlist);
            MediaPlayer.RepeatMode = RepeatMode.RepeatAll;
        }

        public void Play()
        {
            Play(_bgmId);
        }

        public async void Play(int index)
        {
            if (!MediaPlayer.IsInitialized)
            {
                MediaEvents.onInitialized += (initialized) =>
                {
                    if (initialized)
                    {
                        Play(index);
                    }
                };
                return;
            }

            if (_nmpPlaylist == null || _nmpPlaylist.Count == 0)
            {
                GNLog.Error(Issue.MissingContent);
                return;
            }

            IMusic music = null; //SoundManager.Instance.Get(index);
            if (music == null)
            {
                GNLog.Error("BGM시스템에 문제가 있습니다. FALLBACK_BGM_ID(6009)를 찾을 수 없습니다.");
                return;
            }

            MediaMetadata mediaMetadata = new()
            {
                Title = music.Title,
                Artist = music.Artist
            };

            MediaItem mediaItem = new(UriType.DownloadAndPlay, music.Uri, mediaMetadata)
            {
                MetadataType = MetadataType.Custom
            };

            Playlist playlist = new(new List<MediaItem> { mediaItem });

            MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
            MediaPlayer.SetPlaylist(playlist);

            if (MediaPlayer.IsPlaying) MediaPlayer.FadeOut(.8f);
            await WaitAndPlayAsync(.8f, playlist);
        }

        private async UniTask WaitAndPlayAsync(float delay, Playlist playlist)
        {
            await UniTask.Delay((int)(delay * 1000));
            MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
            MediaPlayer.SetPlaylist(playlist);
        }

        public void Next()
        {
            MediaPlayer.Next();
        }

        public void Previous()
        {
            MediaPlayer.Previous();
        }

        public void Release()
        {
            MediaPlayer.Release();
            _playlist = null;
        }

        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public async void FadeOut(float duration = 3)
        {
            MediaPlayer.FadeOut(duration);
            await UniTask.Delay((int)(duration * 1000));
        }

        private Playlist ParsePlaylist(List<IMusic> bgmList)
        {
            List<MediaItem> list = new();

            for (int i = 0; i < bgmList.Count; i++)
            {
                IMusic music = bgmList[i];

                MediaMetadata mediaMetadata = new()
                {
                    Title = music.Title,
                    Artist = music.Artist
                };
                MediaItem mediaItem = new(UriType.DownloadAndPlay, music.Uri, mediaMetadata)
                {
                    MetadataType = MetadataType.Custom
                };
                list.Add(mediaItem);
            }

            GNLog.Info("새로 적용할 플레이리스트에 " + list.Count + "개의 BGM이 있습니다. Playlist: " + bgmList.Count + "개");
            return new Playlist(list);
        }

        public void StartPreview(IMusic music)
        {
            if (music == null)
            {
                GNLog.Error(Issue.MissingContent);
                return;
            }

            MediaMetadata mediaMetadata = new()
            {
                Title = music.Title,
                Artist = music.Artist
            };

            MediaItem mediaItem = new(UriType.DownloadAndPlay, music.Uri, mediaMetadata)
            {
                MetadataType = MetadataType.Custom
            };

            Playlist playlist = new(new List<MediaItem> { mediaItem });

            MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
            MediaPlayer.SetPlaylist(playlist);

            if (MediaPlayer.IsPlaying) MediaPlayer.FadeOut(.8f);
            WaitAndPlayAsync(.8f, playlist).Forget();
        }

        public void StopPreview()
        {
            Release();
        }

        public void PlayWithUri(string uri, bool preview)
        {
            MediaMetadata mediaMetadata = new()
            {
                Title = "BGM",
                Artist = "BGM"
            };

            MediaItem mediaItem = new(UriType.DownloadAndPlay, uri, mediaMetadata)
            {
                MetadataType = MetadataType.Custom
            };

            Playlist playlist = new(new List<MediaItem> { mediaItem });

            MediaPlayer.RepeatMode = RepeatMode.RepeatOne;
            MediaPlayer.SetPlaylist(playlist);

            if (MediaPlayer.IsPlaying) MediaPlayer.FadeOut(.8f);
            WaitAndPlayAsync(.8f, playlist).Forget();
        }

        public IMusic GetCurrentTrack()
        {
            if (_playlist.LogIfNullOrEmpty()) return null;
            return _playlist[_currentTrack];
        }
    }
}
