using System.Text.Json.Serialization;

namespace PlayFabBuddy.PlayFabHelpers.Entities.Accounts
{
    public class MasterPlayerAccountEntity
    {

        [JsonInclude]
        public List<TitlePlayerAccountEntity> PlayerAccounts { get; private set; }

        public string Id { get; set; }

        public MasterPlayerAccountEntity(string id)
        {
            Id = id;
            PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }

        public MasterPlayerAccountEntity(string id, TitlePlayerAccountEntity playerAccount)
        {
            Id = id;
            PlayerAccounts = new List<TitlePlayerAccountEntity>();
        }

        [JsonConstructor]
        public MasterPlayerAccountEntity(string id, List<TitlePlayerAccountEntity> playerAccounts)
        {
            Id = id;
            PlayerAccounts = playerAccounts;
        }

        public bool RemoveTitlePlayerAccount(TitlePlayerAccountEntity account)
        {
            return PlayerAccounts.Remove(account);
        }

        public void AddTitlePlayerAccount(TitlePlayerAccountEntity account)
        {
            PlayerAccounts.Add(account);
        }
    }
}
