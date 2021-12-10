using PlayFabBuddy.PlayFabHelpers.Entities.Accounts;
using System.Text.Json;

namespace PlayFabBuddy.PlayFabHelpers.Util.Repository
{
    public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
    {
        private string configPath;
        private JsonSerializerOptions jsonOptions;
        private List<MasterPlayerAccountEntity>? masterAccountList;

        public LocalMasterPlayerAccountRepository(string pathToConfig)
        {
            this.configPath = pathToConfig;
            this.jsonOptions = new JsonSerializerOptions()
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                WriteIndented = true                
            };
        }

        public List<MasterPlayerAccountEntity> Get()
        {
            string jsonString = File.ReadAllText(this.configPath);

            var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString, jsonOptions);

            if (entityList == null)
            {
                throw new NullReferenceException($"Entity List, is null, Error on Deserialization");
            }

            return entityList;
        }

        public async Task Save(List<MasterPlayerAccountEntity> toSave)
        {
            masterAccountList = toSave;

            using FileStream writeStream = File.Create(this.configPath);
            await JsonSerializer.SerializeAsync<List<MasterPlayerAccountEntity>>(writeStream, toSave, jsonOptions);
            await writeStream.DisposeAsync();
        }
    }
}
