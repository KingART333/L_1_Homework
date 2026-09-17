using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Develop.Runtime.Gameplay.Core
{
    public class GameplayLoopService
    {
        private readonly SequenceGeneratorService _sequenceGenerator;
        private readonly PlayerInputService _playerInput;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameMode _mode;

        private SequenceCheckerService _sequenceChecker;
        private GameplayState _state;

        public GameplayLoopService(
            SequenceGeneratorService sequenceGenerator,
            PlayerInputService playerInput,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            GameMode mode)
        {
            _sequenceGenerator = sequenceGenerator;
            _playerInput = playerInput;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _mode = mode;
        }

        public void Start()
        {
            string sequence = _sequenceGenerator.Generate();

            Debug.Log($"Sequence: {sequence}");

            _sequenceChecker = new SequenceCheckerService(sequence);
            _state = GameplayState.Playing;
        }

        public void Update()
        {
            switch (_state)
            {
                case GameplayState.Playing:
                    UpdatePlaying();
                    break;

                case GameplayState.Win:
                    UpdateWin();
                    break;

                case GameplayState.Lose:
                    UpdateLose();
                    break;

                case GameplayState.Switching:
                    break;
            }
        }

        private void UpdatePlaying()
        {
            if (_playerInput.TryGetSymbol(out char symbol) == false)
                return;

            SequenceCheckResult result = _sequenceChecker.Check(symbol);

            switch (result)
            {
                case SequenceCheckResult.Correct:
                    break;

                case SequenceCheckResult.Failed:
                    Debug.Log("Lose! Press space for restart");
                    _state = GameplayState.Lose;
                    break;

                case SequenceCheckResult.Completed:
                    Debug.Log("Win! Press space for Main Menu");
                    _state = GameplayState.Win;
                    break;
            }
        }

        private void UpdateWin()
        {
            if (Input.GetKeyDown(KeyCode.Space) == false)
                return;

            _state = GameplayState.Switching;
            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
        }

        private void UpdateLose()
        {
            if (Input.GetKeyDown(KeyCode.Space) == false)
                return;

            _state = GameplayState.Switching;
            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(_mode)));
        }
    }
}
