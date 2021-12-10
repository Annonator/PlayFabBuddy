using System.Text.Json.Serialization;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts
{
    public class TitlePlayerAccountEntity
    {
        public string Id { get; init; }

        [JsonIgnore]
        public MasterPlayerAccountEntity? MasterAccount { get; private set; }

        [JsonConstructor]
        public TitlePlayerAccountEntity(string id)
        {
            Id = id;
        }

        public TitlePlayerAccountEntity(string id, MasterPlayerAccountEntity account)
        {
            Id = id;
            AssignMasterAccount(account);
        }

        public void AssignMasterAccount(MasterPlayerAccountEntity account)
        {
            if (MasterAccount != null)
            {
                MasterAccount.RemoveTitlePlayerAccount(this);
            }

            MasterAccount = account;
            MasterAccount.AddTitlePlayerAccount(this);

        }
    }
}
