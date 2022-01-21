namespace PlayFabBuddy.Lib.Entities.Accounts;

public class TitlePlayerAccountEntity
{
    public string? Id { get; init; }

    public MasterPlayerAccountEntity? MasterAccount { get; set; }
}