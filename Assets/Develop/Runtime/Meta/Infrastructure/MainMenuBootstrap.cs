using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using Assets._Project.Develop.Runtime.Meta.Features.Progress;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        public const KeyCode PrintStatusKey = KeyCode.P;
        public const KeyCode ResetProgressKey = KeyCode.R;

        private DIContainer _container;
        private GameModeSelectorService _modeSelector;
        private GameModeInputService _modeInput;
        private ProgressService _progressService;
        private bool _isSwitching;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Initialization gameplay Menu");

            _modeSelector = _container.Resolve<GameModeSelectorService>();
            _modeInput = _container.Resolve<GameModeInputService>();
            _progressService = _container.Resolve<ProgressService>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start gameplay Menu");

            _modeSelector.SelectionConfirmed += OnModeSelected;

            _modeInput.PrintHintIfChanged();

            Debug.Log($"<{PrintStatusKey}> — statistics | <{ResetProgressKey}> — reset progress");
        }

        private void OnDestroy()
        {
            if (_modeSelector != null)
                _modeSelector.SelectionConfirmed -= OnModeSelected;
        }

        private void Update()
        {
            if (_isSwitching)
                return;

            if (_modeInput == null || _modeSelector == null || _progressService == null)
                return;

            _modeInput.Update();
            _modeInput.PrintHintIfChanged();

            if (Input.GetKeyDown(PrintStatusKey))
                _progressService.PrintStatus();
            else if (Input.GetKeyDown(ResetProgressKey))
                ResetProgress();
        }

        private void OnModeSelected(GameMode mode)
        {
            if (_isSwitching)
                return;

            _isSwitching = true;

            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(
                sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(mode)));
        }

        private void ResetProgress()
        {
            if (_progressService.TryResetProgress())
                Debug.Log("Progress reseted");
            else
                Debug.Log("Insufisent funds for reseting progress");
        }
    }
}
