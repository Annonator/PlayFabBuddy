using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class SegmentMasterPlayerAccountRepositorySetting : IRepositorySettings
{
    public string SegmentName { get; set; } = "All Players";
}
