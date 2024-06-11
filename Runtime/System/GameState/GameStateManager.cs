using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Glitch9
{
    public class GameStateManager : MonoSingleton<GameStateManager>
    {
        private const string STATE_BASE_NAME = "GameState";
        public GameStateBase PreviousState { get; set; }
        public GameStateBase CurrentState { get; private set; }

        private Dictionary<string, GameStateBase> _states = new();
        private bool _isInitialized = false;
        private bool _isChangingState = false;

        private void Start()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                FindAllStates();
                SetInitialStateAsync();
            }
        }

        private async void SetInitialStateAsync()
        {
            // 이니셜 타입의 스테이트를 찾는다
            GameStateBase initialState = null;
            foreach (GameStateBase state in _states.Values)
            {
                if (state.Type == GameState.Initial)
                {
                    initialState = state;
                    break;
                }
            }

            if (initialState == null)
            {
                GNLog.Error($"There is no initial state");
                return;
            }

            CurrentState = initialState;
            await GameSceneManager.Instance.LoadScenesAsync(CurrentState.Scenes);

            initialState.OnEnter();
        }

        private void FindAllStates()
        {
            // Use reflection to find all the states
            // Find the initial state first, then find all the other states
            // There is no longer state.IsInitial, so we need to find IGNInitialGameState instead
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsInterface || type.IsAbstract) continue; // Skip interfaces and abstract classes

                // Check if the type is a subclass of the desired base classes
                Type baseType = type;
                while (baseType != null && baseType.Name != STATE_BASE_NAME)
                {
                    baseType = baseType.BaseType;
                }

                if (baseType == null) continue; // Skip if it does not inherit from the desired base classes

                GameStateBase state = (GameStateBase)Activator.CreateInstance(type);
                string stateName = type.Name; // Use the name of the current type

                GNLog.Info($"Found state: {stateName}");
                _states.Add(stateName, state);
            }
        }

        public async void ChangeState<T>() where T : GameStateBase
        {
            GNLog.Info($"Try to changing state to {typeof(T).Name}");

            if (!_states.ContainsKey(typeof(T).Name))
            {
                GNLog.Error($"State {typeof(T).Name} not found in {nameof(_states)}");
                return;
            }

            await ChangeStateAsync(_states[typeof(T).Name]);
        }

        public async void ToggleState<T>(string enterArg = null) where T : GameStateBase
        {
            Type targetStateType = typeof(T);
            string targetStateName = targetStateType.Name;
            GNLog.Info($"Try to toggle state to {targetStateType.Name}");

            if (!_states.ContainsKey(targetStateName))
            {
                GNLog.Error($"State {targetStateName} not found in {nameof(_states)}");
                return;
            }

            GameStateBase targetState = _states[targetStateName];

            if (CurrentState == targetState)
            {
                if (PreviousState != null && PreviousState != targetState)
                {
                    GameStateBase tempState = CurrentState;
                    targetStateName = PreviousState.GetType().Name;
                    await ChangeStateAsync(PreviousState, enterArg);
                    PreviousState = tempState;
                }
                else
                {
                    GNLog.Warning($"There is no valid previous state to toggle to. Setting PreviousState to null.");
                    PreviousState = null;
                }
            }
            else
            {
                PreviousState = CurrentState;
                await ChangeStateAsync(targetState, enterArg);
            }

            GNLog.Info($"Toggled state to {targetStateName}");
        }


        private async UniTask ChangeStateAsync(GameStateBase newStateBase, string enterArg = null)
        {
            while (_isChangingState)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            if (CurrentState == newStateBase) return;

            _isChangingState = true;

            if (!_states.ContainsKey(newStateBase.GetType().Name))
            {
                GNLog.Error($"State {newStateBase} not found in {nameof(_states)}");
                return;
            }

            HashSet<GameScene> scenesToLoad = new(); // Use HashSet for uniqueness
            HashSet<GameScene> scenesToUnload = new();

            // Add scenes from the new state to 'scenesToLoad'
            foreach (GameScene scene in newStateBase.Scenes)
            {
                scenesToLoad.Add(scene);
            }

            // Add scenes from other states to 'scenesToUnload', if not required by the current state
            foreach (GameStateBase state in _states.Values)
            {
                if (state != newStateBase)
                {
                    foreach (GameScene scene in state.Scenes)
                    {
                        if (!scenesToLoad.Contains(scene))
                        {
                            scenesToUnload.Add(scene);
                        }
                    }
                }
            }


            await GameSceneManager.Instance.LoadScenesAsync(scenesToLoad);

            if (CurrentState != null)
            {
                CurrentState.OnExit();

                if (CurrentState.ExitDelay > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(CurrentState.ExitDelay));
                }

                PreviousState = CurrentState;
                CurrentState = newStateBase;
            }

            await GameSceneManager.Instance.UnloadScenesAsync(scenesToUnload);

            CurrentState?.OnEnter(enterArg);
            _isChangingState = false;
        }
    }
}