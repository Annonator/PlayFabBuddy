using System.Text.Json;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Util.Repository
{
    public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
    {
        private string configPath;
        private JsonSerializerOptions jsonOptions;

        public LocalMasterPlayerAccountRepository(string pathToConfig)
        {
            this.configPath = pathToConfig;
            this.jsonOptions = new JsonSerializerOptions()
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
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
            using FileStream writeStream = File.Create(this.configPath);
            await JsonSerializer.SerializeAsync<List<MasterPlayerAccountEntity>>(writeStream, toSave, jsonOptions);
            await writeStream.DisposeAsync();
        }
    }
}
