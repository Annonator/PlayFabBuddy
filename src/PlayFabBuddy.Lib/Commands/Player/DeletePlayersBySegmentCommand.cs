using PlayFab;
using PlayFab.AdminModels;

namespace PlayFabBuddy.Lib.Commands.Player;

/// <summary>
/// Command to delete players by segment
/// </summary>
public class DeletePlayersBySegmentCommand
{
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
}