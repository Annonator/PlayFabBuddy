using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Adapter.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab;

public class PlayStreamAdapter: IPlayStreamAdapter
{
    private PlayerAccountAdapter playerAccountAdapter;

    public PlayStreamAdapter(PlayerAccountAdapter playerAccountAdapter)
    {
        this.playerAccountAdapter = playerAccountAdapter;
    }

    /// <summary>
    /// Get all players in a given segment, identified by <paramref name="segmentId"/>
    /// </summary>
    /// <param name="segmentId">The segment's ID</param>
    /// <returns>All the Master Player Accounts in the segment</returns>
    public async Task<List<MasterPlayerAccountAdapter>> GetPlayersInSegment(string segmentId)
    {
        var getPlayersInSegmentRequest = new GetPlayersInSegmentRequest
        {
            SegmentId = segmentId
        };
        var playersInSegment = await PlayFabAdminAPI.GetPlayersInSegmentAsync(getPlayersInSegmentRequest);

        var accounts = new List<MasterPlayerAccountAdapter>(playersInSegment.Result.ProfilesInSegment);
        foreach (var profile in playersInSegment.Result.PlayerProfiles)
        {
            var account = new MasterPlayerAccountAdapter(profile.PlayerId);
            accounts.Add(account);
        }

        return accounts;
    }

    /// <summary>
    /// Fetches the ID of the "All Players" segment for the Title
    /// </summary>
    /// <returns>The SegmentId</returns>
    /// <exception cref="Exception">When the segment could not be found</exception>
    public async Task<string> GetSegmentById(string segmentName)
    {
        var allSegmentsResponse = await PlayFabAdminAPI.GetAllSegmentsAsync(new GetAllSegmentsRequest());
        var allPlayersSegmentId = "";
        foreach (var segmentResult in allSegmentsResponse.Result.Segments)
        {
            if (segmentResult.Name == segmentName)
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