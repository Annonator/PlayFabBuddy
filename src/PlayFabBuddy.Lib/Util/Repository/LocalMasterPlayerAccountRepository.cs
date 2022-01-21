using System.Text.Json;
using System.Text.Json.Serialization;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Util.Repository;

public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
{
    private readonly string configPath;
    private readonly JsonSerializerOptions jsonOptions;

    public LocalMasterPlayerAccountRepository(string pathToConfig)
    {
        configPath = pathToConfig;
        jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
    }

    public List<MasterPlayerAccountEntity> Get()
    {
        var jsonString = File.ReadAllText(configPath);

        var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString, jsonOptions);

        if (entityList == null) throw new NullReferenceException("Entity List, is null, Error on Deserialization");

        return entityList;
    }

    public async Task Save(List<MasterPlayerAccountEntity> toSave)
    {
        using var writeStream = File.Create(configPath);
        await JsonSerializer.SerializeAsync(writeStream, toSave, jsonOptions);
        await writeStream.DisposeAsync();
    }
}