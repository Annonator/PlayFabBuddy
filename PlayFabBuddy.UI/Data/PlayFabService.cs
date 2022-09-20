using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using System.Net;

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

        /// <summary>
        /// Get a list of all players for a given IP Address
        /// </summary>
        /// <returns></returns>
        private async Task<PlayerData[]> GetPlayers()
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
                        playerData.LastKnownIP = aggregate.MasterPlayerAccount.LastKnownIp;
                        playerData.TitleId = playerAccount.TitleId;

                        playerDataList.Add(playerData);
                    }
                }
            }

            return playerDataList.ToArray();
        }

        /// <summary>
        /// Ban a single player based on their Ip and player Id.
        /// </summary>
        /// <param name="IpAddress">The Ip Address.</param>
        /// <param name="playerId">The Player Id.</param>
        /// <returns></returns>
        private async Task BanSinglePlayerAsync(string ipAddress, string playerId)
        {
            List<MasterPlayerAccountAggregate> playerList = new List<MasterPlayerAccountAggregate>();
            playerList.Add(new MasterPlayerAccountAggregate(new Lib.Entities.Accounts.MasterPlayerAccountEntity { LastKnownIp = ipAddress, Id = playerId }));

            var banUseCase = new BanTitlePlayerAccountsByIpUseCase(_playerAdapter, playerList, IPAddress.Parse(ipAddress), " Test ");

            await banUseCase.ExecuteAsync();
        }

        public Task BanPlayerAsync(string ipAddress, string playerId)
        {
            return BanSinglePlayerAsync(ipAddress, playerId);
        }

        public Task<PlayerData[]> GetForecastAsync()
        {
            var playerDatas = GetPlayers();

            return playerDatas;
        }

    }
}
