using UnityEngine.SceneManagement;

namespace Glitch9
{
    public abstract class GameScene
    {
        /// <summary>
        /// 프로젝트내의 Scene파일이름.
        /// 파일이름을 꼭 확인해야 한다.
        /// </summary>
        public abstract string Name { get; }
        public abstract LoadSceneMode LoadMode { get; }


        public static implicit operator string(GameScene scene) => scene.Name;

        // compare with string
        public static bool operator ==(GameScene scene, string value) => scene.Name == value;
        public static bool operator !=(GameScene scene, string value) => scene.Name != value;

        // equals & hashcode
        public override bool Equals(object obj)
        {
            if (obj is GameScene scene)
            {
                return Name == scene.Name;
            }
            return false;
        }
        public override int GetHashCode() => Name.GetHashCode();
    }
}