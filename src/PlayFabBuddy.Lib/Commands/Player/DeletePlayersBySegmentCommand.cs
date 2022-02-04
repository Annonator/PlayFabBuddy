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

        var getPlayersInSegmentRequest = new GetPlayersInSegmentRequest {
            SegmentId = allPlayersSegmentId
        };
        var playersInSegment = await PlayFabAdminAPI.GetPlayersInSegmentAsync(getPlayersInSegmentRequest);
        playersInSegment.Result.PlayerProfiles.ForEach(player =>
        {
            // TODO: delete player
        });
        
        return await Task.FromResult(true);
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