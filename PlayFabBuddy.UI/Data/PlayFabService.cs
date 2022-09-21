﻿using PlayFabBuddy.Lib.Aggregate;
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
        /// <param name="ipAddress">The Ip Address.</param>
        /// <returns></returns>
        private async Task<PlayerData[]> GetPlayers(string ipAddress)
        {
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
        /// Ban players based on their Ip and player Id.
        /// </summary>
        /// <param name="IpAddress">The Ip Address.</param>
        /// <returns></returns>
        private async Task<bool> BanPlayersAsync(string ipAddress)
        {
            var getPlayerUsecase = new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(ipAddress));

            var playerList = await getPlayerUsecase.ExecuteAsync();

            var banUseCase = new BanTitlePlayerAccountsByIpUseCase(_playerAdapter, playerList.Take(10).ToList(), IPAddress.Parse(ipAddress), " Test Dean");

            return await banUseCase.ExecuteAsync();
        }

        /// <summary>
        /// Ban players based on an IP address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public Task<bool> BanPlayerByIPAsync(string ipAddress)
        {
            return BanPlayersAsync(ipAddress);
        }

        public Task<PlayerData[]> GetPlayersAsync(string ipAddress)
        {
            var playerDatas = GetPlayers(ipAddress);

            return playerDatas;
        }
    }
}
