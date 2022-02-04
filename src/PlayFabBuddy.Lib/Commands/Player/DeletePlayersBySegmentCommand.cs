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
        
        playersInSegment.Result.PlayerProfiles.ForEach(player =>
        {
            // TODO: delete player
        });
        
        return await Task.FromResult(true);
    }

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