﻿using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Lib.UseCases.Player;

/// <summary>
/// Command to delete players by segment
/// </summary>
public class DeletePlayersBySegmentUseCase
{
    private readonly IPlayStreamAdapter playStreamAdapter;
    private readonly IPlayerAccountAdapter playerAccountAdapter;

    public DeletePlayersBySegmentUseCase(IPlayStreamAdapter playStreamAdapter, IPlayerAccountAdapter playerAccountAdapter)
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
    public async Task<(int removedCount, List<MasterPlayerAccountAggregate> playersInSegment)> ExecuteAsync(string segmentName, IProgress<double> progress)
    {
        var segmentId = await playStreamAdapter.GetSegmentById(segmentName);
        var playersInSegment = await playStreamAdapter.GetPlayersInSegment(segmentId);

        var removedCount = await DeletePlayers(playersInSegment, progress);

        return (removedCount, playersInSegment);
    }

    /// <summary>
    /// Deletes a list of players
    /// </summary>
    /// <param name="accounts">The Master Player Accounts to delete</param>
    /// <param name="progress">Used to report back progress during delete operation</param>
    private async Task<int> DeletePlayers(List<MasterPlayerAccountAggregate> accounts, IProgress<double> progress)
    {
        var tasks = new List<Task>();
        foreach (var account in accounts)
        {
            tasks.Add(playerAccountAdapter.Delete(account));
        }

        double percentage = accounts.Count > 0 ? 100 / accounts.Count : 0;

        var totalRemoved = 0;
        while (tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            progress.Report(percentage);
            totalRemoved++;
        }

        return totalRemoved;
    }
}