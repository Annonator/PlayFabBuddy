namespace PlayFabBuddy.Lib.Entities.Accounts;

public class TitlePlayerAccountEntity
{
    public string? Id { get; init; }

    public MasterPlayerAccountEntity? MasterAccount { get; set; }

    public string? TitleId { get; set; }

    public bool? IsBanned { get; set; } = false;
}