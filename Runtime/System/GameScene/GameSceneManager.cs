using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Glitch9
{
    public class GameSceneManager : MonoSingleton<GameSceneManager>
    {
        private const string BASE_SCENE_CLASS_NAME = "GNScene";
        private Dictionary<string, GameScene> _scenes = new();

        private void Start()
        {
            FindAllScenes();
        }

        private void FindAllScenes()
        {
            ReflectionUtils.InstantiateSubclasses<GameScene>((scene) =>
            {
                _scenes.Add(scene.Name, scene);
            });
        }

        public bool IsSceneLoaded(string sceneName)
        {
            if (sceneName.LogIfNullOrWhiteSpace()) return false;
            Scene scene = SceneManager.GetSceneByName(sceneName);
            return scene.isLoaded;
        }

        private async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode, Action onLoaded = null, Action<float> onLoadProgress = null)
        {
            if (IsSceneLoaded(sceneName)) return;
            Debug.Log($"Loading scene {sceneName}");

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);

            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene {sceneName}");
                return;
            }

            while (!asyncLoad.isDone)
            {
                LoadProgress(asyncLoad.progress, onLoadProgress);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            onLoaded?.Invoke();
        }

        private void LoadProgress(float loadProgress, Action<float> onLoadProgress = null)
        {
            float progress = Mathf.Clamp01(loadProgress / 0.9f);
            onLoadProgress?.Invoke(progress);
            //Debug.Log("씬 로딩 중... " + (progress * 100) + "%");
        }

        private async UniTask UnloadSceneAsync(string sceneName, Action onUnloaded = null)
        {
            if (!IsSceneLoaded(sceneName)) return;

            Debug.Log($"Unloading scene {sceneName}");

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            if (asyncUnload == null)
            {
                Debug.LogError($"Failed to unload scene {sceneName}");
                return;
            }

            while (!asyncUnload.isDone)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            onUnloaded?.Invoke();
        }

        public async UniTask LoadScenesAsync(IEnumerable<GameScene> scenes)
        {
            foreach (GameScene scene in scenes)
            {
                await LoadSceneAsync(scene.Name, scene.LoadMode);
            }
        }

        public async UniTask UnloadScenesAsync(IEnumerable<GameScene> scenes)
        {
            foreach (GameScene scene in scenes)
            {
                await UnloadSceneAsync(scene.Name);
            }
        }
    }
}