using System.Net;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Player;

public class BanTitlePlayerAccountsByIpUseCase : UseCase<bool>
{
    private readonly IPAddress _ipToBan;
    private readonly IPlayerAccountAdapter _playerAccountAdapter;
    private readonly List<MasterPlayerAccountAggregate> _playersToBan;
    private readonly string _reason;

    public BanTitlePlayerAccountsByIpUseCase(IPlayerAccountAdapter playerAccountAdapter,
        List<MasterPlayerAccountAggregate> playersToBan, IPAddress ipToBan, string reason)
    {
        _playerAccountAdapter = playerAccountAdapter;
        _ipToBan = ipToBan;
        _reason = reason;
        _playersToBan = playersToBan;
    }

    public async override Task<bool> ExecuteAsync(IProgress<double>? progress = null)
    {
        return await _playerAccountAdapter.BanPlayerByTitlePlayerAccount(_playersToBan,
            _reason + "(provided by PlayFabBuddy)");
    }
}