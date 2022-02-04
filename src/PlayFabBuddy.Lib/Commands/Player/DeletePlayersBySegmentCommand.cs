using PlayFab;
using PlayFab.AdminModels;

namespace PlayFabBuddy.Lib.Commands.Player;

/// <summary>
/// Command to delete players by segment
/// </summary>
public class DeletePlayersBySegmentCommand
{
    private const string AllPlayersSegmentName = "All Players";
    
    public async Task<bool> ExecuteAsync()
    {
        var allPlayersSegmentId = await GetAllPlayersSegmentId();
        var playersInSegment = await GetPlayersInSegment(allPlayersSegmentId);

        await DeletePlayers(playersInSegment);

        return await Task.FromResult(true);
    }

    /// <summary>
    /// Deletes a list of players
    /// </summary>
    /// <param name="players"></param>
    private async static Task DeletePlayers(PlayFabResult<GetPlayersInSegmentResult> players)
    {
        var tasks = new List<Task>();
        foreach (var playerProfile in players.Result.PlayerProfiles)
        {
            tasks.Add(DeleteTitlePLayer(playerProfile));
        }

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Delete a title player
    /// </summary>
    /// <param name="player"></param>
    private async static Task DeleteTitlePLayer(PlayerProfile player)
    {
        var deletePlayerRequest = new DeletePlayerRequest
        {
            PlayFabId = player.PlayerId
        };
        var result = await PlayFabAdminAPI.DeletePlayerAsync(deletePlayerRequest);
    }

    /// <summary>
    /// Get all players in a given segment, identified by <paramref name="segmentId"/>
    /// </summary>
    /// <param name="segmentId">The segment's ID</param>
    /// <returns>Al the players in the segment</returns>
    private async static Task<PlayFabResult<GetPlayersInSegmentResult>> GetPlayersInSegment(string segmentId)
    {
        var getPlayersInSegmentRequest = new GetPlayersInSegmentRequest
        {
            SegmentId = segmentId
        };
        var playersInSegment = await PlayFabAdminAPI.GetPlayersInSegmentAsync(getPlayersInSegmentRequest);
        return playersInSegment;
    }

    /// <summary>
    /// Fetches the ID of the "All Players" segment for the Title
    /// TODO: Good method name???
    /// </summary>
    /// <returns>The SegmentId</returns>
    /// <exception cref="Exception">When the segment could not be found</exception>
    private async static Task<string> GetAllPlayersSegmentId()
    {
        var allSegmentsResponse = await PlayFabAdminAPI.GetAllSegmentsAsync(new GetAllSegmentsRequest());
        var allPlayersSegmentId = "";
        foreach (var segmentResult in allSegmentsResponse.Result.Segments)
        {
            if (segmentResult.Name == AllPlayersSegmentName)
            {
                allPlayersSegmentId = segmentResult.Id;
            }
        }

        if (string.IsNullOrWhiteSpace(allPlayersSegmentId))
        {
            throw new Exception("All Players Segment was not found");
        }

        return allPlayersSegmentId;
    }
}