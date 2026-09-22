using Assets._Project.Develop.Runtime.Gameplay.Core;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets.Develop.Runtime.Gameplay.Core;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private bool _isSwitching;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Initialization gameplay Menu");

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start gameplay Menu");
            Debug.Log("Press 1 — Digits, 2 — Letters, P — Statistics, R — Reset progress");
        }

        private void Update()
        {
            if (_isSwitching)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SwitchToGameplay(GameMode.Digits);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SwitchToGameplay(GameMode.Letters);
            else if (Input.GetKeyDown(KeyCode.P))
                PrintStatus();
            else if (Input.GetKeyDown(KeyCode.R))
                ResetProgress();
        }

        private void SwitchToGameplay(GameMode mode)
        {
            _isSwitching = true;

            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(mode)));
        }

        private void PrintStatus()
        {
            GameProgressService progressService = _container.Resolve<GameProgressService>();
            progressService.PrintStatus();
        }

        private void ResetProgress()
        {
            GameProgressService progressService = _container.Resolve<GameProgressService>();

            if (progressService.TryResetProgress())
                Debug.Log("Progress reseted");
            else
                Debug.Log("Insufisent funds for reseting progress");
        }
    }
}