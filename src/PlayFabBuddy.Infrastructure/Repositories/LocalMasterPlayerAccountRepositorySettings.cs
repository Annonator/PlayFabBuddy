using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class LocalMasterPlayerAccountRepositorySettings : IRepositorySettings
{
    public string DefaultSavePath { get; set; }

    public LocalMasterPlayerAccountRepositorySettings(string defaultSavePath)
    {
        DefaultSavePath = defaultSavePath;
    }
}
