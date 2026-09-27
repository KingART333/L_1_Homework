using System;
using Assets._Project.Develop.Runtime.Gameplay.Core;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.Progress
{
    public class ProgressService
    {
        private readonly GameEconomyConfig _economyConfig;
        private readonly WalletService _walletService;
        private readonly GameStatisticsService _statisticsService;

        public ProgressService(
            ConfigsProviderService configsProviderService,
            WalletService walletService,
            GameStatisticsService statisticsService)
        {
            _economyConfig = configsProviderService.GetConfig<GameEconomyConfig>();
            _walletService = walletService;
            _statisticsService = statisticsService;
        }

        public void PrintStatus()
        {
            int gold = _walletService.GetCurrency(CurrencyTypes.Gold).Value;

            Debug.Log($"Wins: {_statisticsService.WinsCount} | Losses: {_statisticsService.LossesCount} | Gold: {gold}");
        }

        public bool TryResetProgress()
        {
            if (_walletService.Enough(CurrencyTypes.Gold, _economyConfig.ResetProgressCost) == false)
                return false;

            _walletService.Spend(CurrencyTypes.Gold, _economyConfig.ResetProgressCost);

            _statisticsService.ResetStatistics();

            return true;
        }

        public void RewardForWin() => _walletService.Add(CurrencyTypes.Gold, _economyConfig.WinGoldReward);

        public void PenaltyForLoss()
        {
            int currentGold = _walletService.GetCurrency(CurrencyTypes.Gold).Value;
            int amountToSpend = Math.Min(currentGold, _economyConfig.LoseGoldPenalty);

            if (amountToSpend > 0)
                _walletService.Spend(CurrencyTypes.Gold, amountToSpend);
        }
    }
}
