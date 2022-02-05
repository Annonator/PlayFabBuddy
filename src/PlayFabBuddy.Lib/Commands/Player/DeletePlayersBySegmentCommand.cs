using PlayFabBuddy.Lib.Adapter.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.Commands.Player;

/// <summary>
/// Command to delete players by segment
/// </summary>
public class DeletePlayersBySegmentCommand
{
    private readonly IPlayStreamAdapter playStreamAdapter;
    private readonly IPlayerAccountAdapter playerAccountAdapter;

    public DeletePlayersBySegmentCommand(IPlayStreamAdapter playStreamAdapter, IPlayerAccountAdapter playerAccountAdapter)
    {
        this.playStreamAdapter = playStreamAdapter;
        this.playerAccountAdapter = playerAccountAdapter;
    }

    public async Task<bool> ExecuteAsync()
    {
        var allPlayersSegmentId = await this.playStreamAdapter.GetAllPlayersSegmentId();
        var playersInSegment = await this.playStreamAdapter.GetPlayersInSegment(allPlayersSegmentId);

        await DeletePlayers(playersInSegment);

        return await Task.FromResult(true);
    }
    
    /// <summary>
    /// Deletes a list of players
    /// </summary>
    /// <param name="accounts"></param>
    private async Task DeletePlayers(List<MasterPlayerAccountAdapter> accounts)
    {
        var tasks = new List<Task>();
        foreach (var account in accounts)
        {
            tasks.Add(this.playerAccountAdapter.Delete(account.MainAccount));
        }

        await Task.WhenAll(tasks);
    }
}