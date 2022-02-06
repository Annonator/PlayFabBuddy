namespace PlayFabBuddy.Lib.Entities.Accounts;

/// <summary>
/// Represents a list of Tiles an Account has played in
/// </summary>
public class PlayedTitlesListEntity
{
    /// <summary>
    /// A list of PlayFab Title IDs
    /// </summary>
    public string[] TitleIds { get; }
    
    public PlayedTitlesListEntity(string[] titleIds)
    {
        this.TitleIds = titleIds;
    }

    /// <summary>
    /// Returns whether there is more than one Title in the list
    /// </summary>
    /// <returns>Whether there is more than one Title in the list</returns>
    public bool MoreThanOne()
    {
        return this.TitleIds.Length > 1;
    }
}
