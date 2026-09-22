using System;
using Assets._Project.Develop.Runtime.Gameplay.Configs;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class GameProgressService
    {
        private readonly GameEconomyConfig _economyConfig;
        private readonly WalletService _walletService;
        private readonly GameStatisticsService _statisticsService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public GameProgressService(
            ConfigsProviderService configsProviderService,
            WalletService walletService,
            GameStatisticsService statisticsService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _economyConfig = configsProviderService.GetConfig<GameEconomyConfig>();
            _walletService = walletService;
            _statisticsService = statisticsService;
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void RegisterWin()
        {
            _walletService.Add(CurrencyTypes.Gold, _economyConfig.WinGoldReward);
            _statisticsService.RegisterWin();

            Save();
        }

        public void RegisterLoss()
        {
            int currentGold = _walletService.GetCurrency(CurrencyTypes.Gold).Value;
            int amountToSpend = Math.Min(currentGold, _economyConfig.LoseGoldPenalty);

            if (amountToSpend > 0)
                _walletService.Spend(CurrencyTypes.Gold, amountToSpend);

            _statisticsService.RegisterLoss();

            Save();
        }

        public bool TryResetProgress()
        {
            if (_walletService.Enough(CurrencyTypes.Gold, _economyConfig.ResetProgressCost) == false)
                return false;

            _walletService.Spend(CurrencyTypes.Gold, _economyConfig.ResetProgressCost);
            _statisticsService.ResetProgress();

            Save();

            return true;
        }

        public void PrintStatus()
        {
            int gold = _walletService.GetCurrency(CurrencyTypes.Gold).Value;

            Debug.Log($"Wins: {_statisticsService.WinsCount} | Losses: {_statisticsService.LossesCount} | Gold: {gold}");
        }

        private void Save() => _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
    }
}