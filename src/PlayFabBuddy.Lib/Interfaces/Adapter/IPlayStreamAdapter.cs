using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Interfaces.Adapter;

public interface IPlayStreamAdapter
{
    /// <summary>
    /// All Players
    /// </summary>
    public const string AllPlayersSegmentName = "All Players";
    
    /// <summary>
    /// Last login greater than 30 days ago
    /// </summary>
    public const string LapsedPlayersSegmentName = "Lapsed Players";
    
    /// <summary>
    /// Total value to date in USD greater than $0.00
    /// </summary>
    public const string PayersSegmentName = "Payers";
    
    /// <summary>
    /// First login less than 14 days ago AND First login greater than or equal to 7 days ago AND Last login less than 7 days ago
    /// </summary>
    public const string WeekTwoActivePlayersSegmentName = "Week Two Active Players";

    /// <summary>
    /// Get all players in a given segment, identified by <paramref name="segmentId"/>
    /// </summary>
    /// <param name="segmentId">The segment's ID</param>
    /// <returns>All the Master Player Accounts in the segment</returns>
    public Task<List<MasterPlayerAccountEntity>> GetPlayersInSegment(string segmentId);

    /// <summary>
    /// Fetches the ID of the "All Players" segment for the Title
    /// </summary>
    /// <returns>The SegmentId</returns>
    /// <exception cref="Exception">When the segment could not be found</exception>
    public Task<string> GetSegmentById(string segmentName);
}