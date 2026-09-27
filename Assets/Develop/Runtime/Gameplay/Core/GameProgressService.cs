using Assets._Project.Develop.Runtime.Meta.Features.Progress;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;

namespace Assets._Project.Develop.Runtime.Gameplay.Core
{
    public class GameProgressService
    {
        private readonly ProgressService _progressService;
        private readonly GameStatisticsService _statisticsService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        public GameProgressService(
            ProgressService progressService,
            GameStatisticsService statisticsService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _progressService = progressService;
            _statisticsService = statisticsService;
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void RegisterWin()
        {
            _progressService.RewardForWin();
            _statisticsService.RegisterWin();

            Save();
        }

        public void RegisterLoss()
        {
            _progressService.PenaltyForLoss();
            _statisticsService.RegisterLoss();

            Save();
        }

        private void Save() => _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
    }
}
