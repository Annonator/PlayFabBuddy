using System.Text.Json.Serialization;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts
{
    public class TitlePlayerAccountEntity
    {
        public string? Id { get; init; }

        public MasterPlayerAccountEntity? MasterAccount { get; set; }
    }
}
