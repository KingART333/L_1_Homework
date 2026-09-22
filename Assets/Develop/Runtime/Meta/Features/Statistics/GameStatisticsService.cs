using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;

namespace Assets._Project.Develop.Runtime.Meta.Features.Statistics
{
    public class GameStatisticsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private int _winsCount;
        private int _lossesCount;

        public GameStatisticsService(PlayerDataProvider playerDataProvider)
        {
            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
        }

        public int WinsCount => _winsCount;
        public int LossesCount => _lossesCount;

        public void RegisterWin() => _winsCount++;

        public void RegisterLoss() => _lossesCount++;

        public void ResetProgress()
        {
            _winsCount = 0;
            _lossesCount = 0;
        }

        public void ReadFrom(PlayerData data)
        {
            _winsCount = data.WinsCount;
            _lossesCount = data.LossesCount;
        }

        public void WriteTo(PlayerData data)
        {
            data.WinsCount = _winsCount;
            data.LossesCount = _lossesCount;
        }
    }
}