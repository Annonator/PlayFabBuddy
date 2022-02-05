using PlayFabBuddy.Lib.Adapter.Accounts;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayStreamAdapter
{
    public const string AllPlayersSegmentName = "All Players";

    /// <summary>
    /// Get all players in a given segment, identified by <paramref name="segmentId"/>
    /// </summary>
    /// <param name="segmentId">The segment's ID</param>
    /// <returns>All the Master Player Accounts in the segment</returns>
    public Task<List<MasterPlayerAccountAdapter>> GetPlayersInSegment(string segmentId);

    /// <summary>
    /// Fetches the ID of the "All Players" segment for the Title
    /// </summary>
    /// <returns>The SegmentId</returns>
    /// <exception cref="Exception">When the segment could not be found</exception>
    public Task<string> GetAllPlayersSegmentId();
}