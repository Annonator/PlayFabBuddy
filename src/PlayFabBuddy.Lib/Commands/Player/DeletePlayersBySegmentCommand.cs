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

    /// <summary>
    /// Deletes all Master Player Accounts from a given segment, identified by <paramref name="segmentName"/>.
    /// Reports back progress via <paramref name="progress"/>
    /// </summary>
    /// <param name="segmentName">The segment name from which Master Player Accounts shall be deleted from</param>
    /// <param name="progress">A progress object to report progress on</param>
    /// <returns>The number of deleted Master Player Accounts</returns>
    public async Task<(int removedCount, List<MasterPlayerAccountAdapter> playersInSegment)> ExecuteAsync(string segmentName, IProgress<double> progress)
    {
        var segmentId = await this.playStreamAdapter.GetSegmentById(segmentName);
        var playersInSegment = await this.playStreamAdapter.GetPlayersInSegment(segmentId);

        var removedCount = await DeletePlayers(playersInSegment, progress);

        return (removedCount, playersInSegment);
    }

    /// <summary>
    /// Deletes a list of players
    /// </summary>
    /// <param name="accounts">The Master Player Accounts to delete</param>
    /// <param name="progress">Used to report back progress during delete operation</param>
    private async Task<int> DeletePlayers(List<MasterPlayerAccountAdapter> accounts, IProgress<double> progress)
    {
        var tasks = new List<Task>();
        foreach (var account in accounts)
        {
            tasks.Add(this.playerAccountAdapter.Delete(account.MainAccount));
        }

        double percentage = (accounts.Count > 0) ? 100 / accounts.Count : 0;

        var totalRemoved = 0;
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            await finishedTask;
            progress.Report(percentage);
            totalRemoved++;
        }

        return totalRemoved;
    }
}