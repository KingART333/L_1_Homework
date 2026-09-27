using Assets._Project.Develop.Runtime.Gameplay.Core;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Process of registration of services on menu scene");

            container.RegisterAsSingle(c => new GameModeSelectorService(
                c.Resolve<ConfigsProviderService>().GetConfig<GameplayConfig>().GetAvailableModes()));

            container.RegisterAsSingle(c => new GameModeInputService(c.Resolve<GameModeSelectorService>()));
        }
    }
}
