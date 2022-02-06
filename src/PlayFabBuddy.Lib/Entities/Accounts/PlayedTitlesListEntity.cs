namespace PlayFabBuddy.Lib.Entities.Accounts;

public class PlayedTitlesListEntity
{
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