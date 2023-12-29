using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;

namespace PlayFabBuddy.UI.Data;

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
    ///     Get a list of all players for a given IP Address
    /// </summary>
    /// <param name="ipAddress">The Ip Address.</param>
    /// <returns></returns>
    private async Task<PlayerData[]> GetPlayers(string ipAddress)
    {
        var getPlayerUsecase = new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(ipAddress));

        var playerList = await getPlayerUsecase.ExecuteAsync();

        var playerDataList = new List<PlayerData>();

        foreach (var aggregate in playerList)
        {
            var playerData = new PlayerData();

            if (aggregate.MasterPlayerAccount.PlayerAccounts != null)
            {
                foreach (var playerAccount in aggregate.MasterPlayerAccount.PlayerAccounts)
                {
                    playerData.Id = playerAccount.Id;
                    playerData.IsBanned = playerAccount.IsBanned;
                    playerData.LastKnownIP = aggregate.MasterPlayerAccount.LastKnownIp;
                    playerData.TitleId = playerAccount.TitleId;
                    playerData.CustomId = aggregate.MasterPlayerAccount.CustomId;
                    playerData.MasterPlayerAccountId = aggregate.MasterPlayerAccount.Id;

                    playerDataList.Add(playerData);
                }
            }
        }

        return playerDataList.ToArray();
    }

    /// <summary>
    ///     Ban players based on their Ip and player Id.
    /// </summary>
    /// <param name="IpAddress">The Ip Address.</param>
    /// <returns></returns>
    private async Task<bool> BanPlayers(string ipAddress)
    {
        var getPlayerUsecase = new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(ipAddress));

        var playerList = await getPlayerUsecase.ExecuteAsync();

        var banUseCase =
            new BanTitlePlayerAccountsByIpUseCase(_playerAdapter, playerList, IPAddress.Parse(ipAddress), " Test Dean");

        return await banUseCase.ExecuteAsync();
    }

    /// <summary>
    ///     Ban a single player only based on their masterPlayerAccountId
    /// </summary>
    /// <param name="ipAddress">The Ip Address.</param>
    /// <param name="masterPlayerAccountId"></param>
    /// <param name="customId"></param>
    /// <param name="titleId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<bool> BanSinglePlayer(string ipAddress, string masterPlayerAccountId, string customId,
        string titleId, string id)
    {
        // Build up the Player Account Object
        var aggregateList = new List<MasterPlayerAccountAggregate>();

        var player = new MasterPlayerAccountAggregate(masterPlayerAccountId) {
            MasterPlayerAccount = { Id = masterPlayerAccountId, CustomId = customId, LastKnownIp = ipAddress }
        };

        var titlePlayerAccount = new TitlePlayerAccountEntity { TitleId = titleId, Id = id };

        if (player.MasterPlayerAccount.PlayerAccounts is not null)
        {
            player.MasterPlayerAccount.PlayerAccounts.Add(titlePlayerAccount);
        }

        aggregateList.Add(player);

        // Call API and ban the individual player
        var banUseCase =
            new BanTitlePlayerAccountsByIpUseCase(_playerAdapter, aggregateList, IPAddress.Parse(ipAddress),
                " Test Dean");

        return await banUseCase.ExecuteAsync();
    }

    /// <summary>
    ///     Ban players based on an IP address
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public Task<bool> BanPlayerByIpAsync(string ipAddress)
    {
        return BanPlayers(ipAddress);
    }

    /// <summary>
    ///     Ban a single player
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <param name="masterPlayerAccountId"></param>
    /// <param name="customId"></param>
    /// <param name="titleId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<bool> BanSinglePlayerAsync(string ipAddress, string masterPlayerAccountId, string customId,
        string titleId, string id)
    {
        return BanSinglePlayer(ipAddress, masterPlayerAccountId, customId, titleId, id);
    }

    /// <summary>
    ///     Get all the players for a given IP Address
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public Task<PlayerData[]> GetPlayersAsync(string ipAddress)
    {
        return GetPlayers(ipAddress);
    }
}