using PlayFab;
using PlayFab.AdminModels;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab.Admin;

public class PlayStreamAdapter : IPlayStreamAdapter
{
    private readonly PlayFabAdminInstanceAPI _playFabAdminInstanceApi;

    public PlayStreamAdapter(PlayFabAdminInstanceAPI playFabAdminInstanceApi)
    {
        this._playFabAdminInstanceApi = playFabAdminInstanceApi;
    }

    /// <summary>
    /// Get all players in a given segment, identified by <paramref name="segmentId"/>
    /// </summary>
    /// <param name="segmentId">The segment's ID</param>
    /// <returns>All the Master Player Accounts in the segment</returns>
    public async Task<List<MasterPlayerAccountAggregate>> GetPlayersInSegment(string segmentId)
    {
        var getPlayersInSegmentRequest = new GetPlayersInSegmentRequest
        {
            SegmentId = segmentId
        };
        var playersInSegment = await this._playFabAdminInstanceApi.GetPlayersInSegmentAsync(getPlayersInSegmentRequest);

        var accounts = new List<MasterPlayerAccountAggregate>(playersInSegment.Result.ProfilesInSegment);
        foreach (var profile in playersInSegment.Result.PlayerProfiles)
        {
            var account = new MasterPlayerAccountAggregate(profile.PlayerId);

            var customId = profile.LinkedAccounts.Find(x => x.Platform == LoginIdentityProvider.Custom);
            if (customId != null)
            {
                account.MasterPlayerAccount.CustomId = customId.PlatformUserId;
            }

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
        var allSegmentsResponse = await _playFabAdminInstanceApi.GetAllSegmentsAsync(new GetAllSegmentsRequest());
        var segmentId = "";
        foreach (var segmentResult in allSegmentsResponse.Result.Segments)
        {
            if (segmentResult.Name == segmentName)
            {
                segmentId = segmentResult.Id;
            }
        }

        if (string.IsNullOrWhiteSpace(segmentId))
        {
            throw new Exception($"A Segment named \"{segmentName}\" was not found");
        }

        return segmentId;
    }
}