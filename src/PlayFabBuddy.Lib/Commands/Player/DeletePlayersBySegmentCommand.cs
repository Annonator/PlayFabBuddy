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

    public async Task<bool> ExecuteAsync(string segmentName, IProgress<double> progress)
    {
        var allPlayersSegmentId = await this.playStreamAdapter.GetSegmentById(segmentName);
        var playersInSegment = await this.playStreamAdapter.GetPlayersInSegment(allPlayersSegmentId);

        await DeletePlayers(playersInSegment, progress);

        return await Task.FromResult(true);
    }

    /// <summary>
    /// Deletes a list of players
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="progress"></param>
    private async Task DeletePlayers(List<MasterPlayerAccountAdapter> accounts, IProgress<double> progress)
    {
        var tasks = new List<Task>();
        foreach (var account in accounts)
        {
            tasks.Add(this.playerAccountAdapter.Delete(account.MainAccount));
        }

        double percentage = (accounts.Count > 0) ? 100 / accounts.Count : 0;
        
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            await finishedTask;
            progress.Report(percentage);
        }

        
        await Task.WhenAll(tasks);
    }
}