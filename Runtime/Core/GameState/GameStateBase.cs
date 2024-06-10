namespace Glitch9
{
    /// <summary>
    /// 게임 스테이트(상태)의 베이스 클래스입니다.
    /// </summary>
    public abstract class GameStateBase
    {
        private GameScene[] _scenes;
        public GameScene[] Scenes => _scenes ??= GetScenes();
        protected abstract GameScene[] GetScenes();
        public abstract float ExitDelay { get; }
        public abstract GameState Type { get; }
        public abstract void OnEnter(string arg = null);
        public abstract void OnExit();

        /// <summary>
        /// 특수한 케이스 / Exit 시 무조건 인게임 메뉴로 돌아가야 하는 경우 true로 설정합니다.
        /// </summary>
        public virtual bool BackToInGameMenuOnExit => false;

        protected GameStateBase() { }
    }
}