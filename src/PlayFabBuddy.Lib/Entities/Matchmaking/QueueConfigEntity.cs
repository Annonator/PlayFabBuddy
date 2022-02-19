namespace PlayFabBuddy.Lib.Entities.Matchmaking;

public class QueueConfigEntity
{
    public string? Name { get; set; }
    public uint MinMatchSize { get; set; }
    public uint MaxMatchSize { get; set; }

    // statistics visible to palyer
    public bool ShowNumbersOfPlayersMatching { get; set; } = false;
    public bool ShowTimeToMatch { get; set; } = false;
}
