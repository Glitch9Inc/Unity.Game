using System.Collections.Generic;

namespace Glitch9.Game
{
    public interface IBGMPlayer
    {
        void Initialize();
        void Prepare(List<ISoundTrack> bgmList);
        void Play();
        void Play(int index);
        void Next();
        void Previous();
        void SetVolume(float volume);
        void Release();
        void StartPreview(ISoundTrack music);
        void StartPreview(int trackNumber);
        void StopPreview();
        void PlayWithUri(string uri, bool preview);
        void FadeOut(float duration = 3f);
        ISoundTrack GetCurrentTrack();
        int CurrentMediaIndex { get; }
    }
}