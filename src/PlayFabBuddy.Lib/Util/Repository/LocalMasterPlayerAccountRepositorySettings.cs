namespace PlayFabBuddy.Lib.Util.Repository;

public class LocalMasterPlayerAccountRepositorySettings : IRepositorySettings
{
    public string DefaultSavePath { get; set; }

    public LocalMasterPlayerAccountRepositorySettings(string defaultSavePath)
    {
        DefaultSavePath = defaultSavePath;
    }
}
