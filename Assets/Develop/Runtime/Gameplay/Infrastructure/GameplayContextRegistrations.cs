using Assets._Project.Develop.Runtime.Gameplay.Core;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets.Develop.Runtime.Gameplay.Core;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            container.RegisterAsSingle(c =>
                new SequenceGeneratorService(c.Resolve<ConfigsProviderService>(), args.Mode));

            container.RegisterAsSingle(c => new PlayerInputService());

            container.RegisterAsSingle(c => new GameplayLoopService(
                c.Resolve<SequenceGeneratorService>(),
                c.Resolve<PlayerInputService>(),
                c.Resolve<SceneSwitcherService>(),
                c.Resolve<ICoroutinesPerformer>(),
                c.Resolve<GameProgressService>(),
                args.Mode));
        }
    }
}