using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(GameMode mode)
        {
            Mode = mode;
        }

        public GameMode Mode { get; }
    }

}
