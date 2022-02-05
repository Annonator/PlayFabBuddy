using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class PlayStreamAdapter: IPlayStreamAdapter
{
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
            if (segmentResult.Name == IPlayStreamAdapter.AllPlayersSegmentName)
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