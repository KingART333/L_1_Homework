using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class GameplayRestartService
    {
        public const KeyCode ContinueKey = KeyCode.Space;

        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameMode _mode;

        private bool _continueRequested;

        public GameplayRestartService(
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            GameMode mode)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _mode = mode;
        }

        public bool CanContinue => _continueRequested;

        public void Update()
        {
            if (_continueRequested)
                return;

            if (Input.GetKeyDown(ContinueKey) == false)
                return;

            _continueRequested = true;
        }

        public void Continue(GameRoundResult roundResult)
        {
            _continueRequested = false;

            switch (roundResult)
            {
                case GameRoundResult.Win:
                    _coroutinesPerformer.StartPerform(
                        _sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
                    break;

                case GameRoundResult.Loss:
                    _coroutinesPerformer.StartPerform(
                        _sceneSwitcherService.ProcessSwitchTo(
                            Scenes.Gameplay,
                            new GameplayInputArgs(_mode)));
                    break;

                default:
                    throw new InvalidOperationException($"Cannot continue round with result {roundResult}");
            }
        }
    }
}
