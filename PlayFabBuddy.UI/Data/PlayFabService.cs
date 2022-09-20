using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace PlayFabBuddy.UI.Data
{
    public class PlayFabService
    {
        private readonly IDataExplorerAdapter _dataAdapter;
        private readonly IPlayerAccountAdapter _playerAdapter;

        public PlayFabService(IDataExplorerAdapter dataAdapter, IPlayerAccountAdapter playerAccountAdapter)
        {
            _dataAdapter = dataAdapter;
            _playerAdapter = playerAccountAdapter;
        }

        public async Task<PlayerData[]> Test()
        {
            string ipAddress = "80.130.46.114";

            var getPlayerUsecase = new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(ipAddress));

            var playerList = await getPlayerUsecase.ExecuteAsync();

            List<PlayerData> playerDataList = new List<PlayerData>();

            foreach (var aggregate in playerList)
            {
                PlayerData playerData = new PlayerData();

                if (aggregate.MasterPlayerAccount.PlayerAccounts != null)
                {
                    foreach (var playerAccount in aggregate.MasterPlayerAccount.PlayerAccounts)
                    {
                        playerData.Id = playerAccount.Id;
                        playerData.IsBanned = playerAccount.IsBanned;

                        playerDataList.Add(playerData);
                    }
                }
            }

            return playerDataList.ToArray();
        }

        public Task<PlayerData[]> GetForecastAsync()
        {
            var playerDatas = Test();

            return playerDatas;

            // return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //}).ToArray());
        }
    }
}
