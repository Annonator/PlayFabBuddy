using System.Text.Json;
using System.Text.Json.Serialization;
using PlayFabBuddy.Lib.Entities.Accounts;

namespace PlayFabBuddy.Lib.Util.Repository;

public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
{
    private readonly string _configPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public LocalMasterPlayerAccountRepository(string pathToConfig)
    {
        _configPath = pathToConfig;
        _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
    }

    public List<MasterPlayerAccountEntity> Get()
    {
        var jsonString = File.ReadAllText(_configPath);

        var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString, _jsonOptions);

        if (entityList == null)
        {
            throw new NullReferenceException("Entity List, is null, Error on Deserialization");
        }

        return entityList;
    }

    public async Task Save(List<MasterPlayerAccountEntity> toSave)
    {
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, toSave, _jsonOptions);
        await writeStream.DisposeAsync();
    }

    public async Task Append(List<MasterPlayerAccountEntity> toAppend)
    {
        var oldUsers = Get();
        oldUsers.AddRange(toAppend);
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, oldUsers, _jsonOptions);
        await writeStream.DisposeAsync();
    }
}