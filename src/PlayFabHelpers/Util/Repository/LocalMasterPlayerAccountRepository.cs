using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using System.Text.Json;

namespace PlayFabBuddy.PlayFabHelpers.Util.Repository
{
    public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
    {
        private string configPath;
        private List<MasterPlayerAccountEntity>? MasterAccountList;

        public LocalMasterPlayerAccountRepository(string pathToConfig)
        {
            this.configPath = pathToConfig;
        }

        public List<MasterPlayerAccountEntity> Get()
        {
            string jsonString = File.ReadAllText(this.configPath);

            var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString);

            if (entityList == null)
            {
                throw new NullReferenceException($"Entity List, is null, Error on Deserialization");
            }

            return entityList;
        }

        public async void Save(List<MasterPlayerAccountEntity> toSave)
        {
            MasterAccountList = toSave;

            using FileStream writeStream = File.Create(this.configPath);
            await JsonSerializer.SerializeAsync<List<MasterPlayerAccountEntity>>(writeStream, toSave);
            await writeStream.DisposeAsync();
        }
    }
}
