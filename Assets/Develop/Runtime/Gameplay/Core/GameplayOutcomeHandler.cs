using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class GameplayOutcomeHandler
    {
        private readonly GameProgressService _progressService;

        public GameplayOutcomeHandler(GameProgressService progressService)
        {
            _progressService = progressService;
        }

        public event Action WinHappened;
        public event Action LossHappened;

        public void Win()
        {
            _progressService.RegisterWin();

            Debug.Log("Win!");

            WinHappened?.Invoke();
        }

        public void Lose()
        {
            _progressService.RegisterLoss();

            Debug.Log("Lose!");

            LossHappened?.Invoke();
        }
    }
}
