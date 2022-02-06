namespace PlayFabBuddy.Lib.Entities.Accounts;

/// <summary>
/// Represents a list of tiles an Account has Played in
/// </summary>
public class PlayedTitlesListEntity
{
    /// <summary>
    /// AA list of PlayFab Title IDs
    /// </summary>
    public string[] TitleIds { get; }
    
    public PlayedTitlesListEntity(string[] titleIds)
    {
        this.TitleIds = titleIds;
    }

    /// <summary>
    /// returns whether there are more than one title in the list
    /// </summary>
    /// <returns>Whether there are more than one title in the list</returns>
    public bool MoreThanOne()
    {
        return this.TitleIds.Length > 1;
    }
}