using Assets._Project.Develop.Runtime.Meta.Features.GameModes;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class GameplayLoopService
    {
        private readonly SequenceGeneratorService _sequenceGenerator;
        private readonly PlayerInputService _playerInput;
        private readonly GameplayOutcomeHandler _outcomeHandler;
        private readonly GameplayRestartService _restartService;

        private SequenceCheckerService _sequenceChecker;
        private GameMode _mode;
        private GameplayState _state;
        private GameRoundResult _roundResult;

        public GameplayLoopService(
            SequenceGeneratorService sequenceGenerator,
            PlayerInputService playerInput,
            GameplayOutcomeHandler outcomeHandler,
            GameplayRestartService restartService,
            GameMode mode)
        {
            _sequenceGenerator = sequenceGenerator;
            _playerInput = playerInput;
            _outcomeHandler = outcomeHandler;
            _restartService = restartService;
            _mode = mode;

            _outcomeHandler.WinHappened += OnWin;
            _outcomeHandler.LossHappened += OnLoss;
        }

        public void Start()
        {
            string sequence = _sequenceGenerator.Generate(_mode);

            Debug.Log($"Mode: {_mode} | Sequence: {sequence}");

            _sequenceChecker = new SequenceCheckerService(sequence);
            _roundResult = GameRoundResult.None;
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
                    UpdateEnded();
                    break;

                case GameplayState.Lose:
                    UpdateEnded();
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
                    _outcomeHandler.Lose();
                    break;

                case SequenceCheckResult.Completed:
                    _outcomeHandler.Win();
                    break;
            }
        }

        private void UpdateEnded()
        {
            _restartService.Update();

            if (_restartService.CanContinue == false)
                return;

            _restartService.Continue(_roundResult);

            _state = GameplayState.Switching;
        }

        private void OnWin()
        {
            Debug.Log($"Win! Press {GameplayRestartService.ContinueKey} for main menu");

            _roundResult = GameRoundResult.Win;
            _state = GameplayState.Win;
        }

        private void OnLoss()
        {
            Debug.Log($"Lose! Press {GameplayRestartService.ContinueKey} for restart");

            _roundResult = GameRoundResult.Loss;
            _state = GameplayState.Lose;
        }
    }
}
