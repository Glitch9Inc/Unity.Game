using System.Collections.Generic;

namespace Glitch9.Game
{
    public interface IAudioPlayer
    {
        void Initialize();
        void Prepare(List<IMusic> bgmList);
        void Play();
        void Play(int index);
        void Next();
        void Previous();
        void SetVolume(float volume);
        void Release();
        void StartPreview(IMusic music);
        void StopPreview();
        void PlayWithUri(string uri, bool preview);
        void FadeOut(float duration = 3f);
        IMusic GetCurrentTrack();
        int CurrentMediaIndex { get; }
    }
}