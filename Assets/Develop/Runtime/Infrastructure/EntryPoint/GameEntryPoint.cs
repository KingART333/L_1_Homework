using Assets._Project.Develop.Runtime.Gameplay.Core;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Project Start, Setup settings");

            SetupAppSettings();

            DIContainer projectContainer = new DIContainer();

            Debug.Log("whole project process Registration");

            ProjectContextRegistrations.Process(projectContainer);

            projectContainer.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(projectContainer));
        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();

            loadingScreen.Show();

            Debug.Log("Initializing services");

            yield return container.Resolve<ConfigsProviderService>().LoadAsync();

            // резолвим GameProgressService, чтобы WalletService и GameStatisticsService
            // успели зарегистрироваться как reader/writer у PlayerDataProvider
            container.Resolve<GameProgressService>();

            PlayerDataProvider playerDataProvider = container.Resolve<PlayerDataProvider>();

            bool saveExists = false;
            yield return playerDataProvider.Exists(result => saveExists = result);

            if (saveExists)
            {
                yield return playerDataProvider.Load();
            }
            else
            {
                playerDataProvider.Reset();
                yield return playerDataProvider.Save();
            }

            yield return new WaitForSeconds(1f);

            Debug.Log("Ending initializing services");

            loadingScreen.Hide();

            yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
        }
    }
}