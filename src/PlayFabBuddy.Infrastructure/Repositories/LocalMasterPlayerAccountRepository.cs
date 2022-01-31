using System.Text.Json;
using System.Text.Json.Serialization;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountEntity>
{
    private readonly string _configPath;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly List<MasterPlayerAccountEntity> _cache;
    private DateTime _lastUpdate;

    public LocalMasterPlayerAccountRepository(LocalMasterPlayerAccountRepositorySettings settings)
    {
        _configPath = settings.DefaultSavePath;
        _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        _cache = new List<MasterPlayerAccountEntity>();
        _lastUpdate = DateTime.MinValue;
    }

    public List<MasterPlayerAccountEntity> Get()
    {
        if (!File.Exists(_configPath))
        {
            File.CreateText(_configPath).Close();
            return _cache;
        }

        var fileUpdateStamp = File.GetLastWriteTimeUtc(_configPath);

        if (_lastUpdate >= fileUpdateStamp)
        {
            return _cache;
        }

        _lastUpdate = fileUpdateStamp;

        var jsonString = File.ReadAllText(_configPath);

        var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString, _jsonOptions);

        if (entityList == null)
        {
            throw new NullReferenceException("Entity List, is null, Error on Deserialization");
        }

        _cache.Clear();
        _cache.AddRange(entityList);

        return _cache;

    }

    public async Task Save(List<MasterPlayerAccountEntity> toSave)
    {
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, toSave, _jsonOptions);
        _cache.Clear();
        _cache.AddRange(toSave);
        await writeStream.DisposeAsync();
    }

    public async Task Append(List<MasterPlayerAccountEntity> toAppend)
    {
        var oldUsers = Get();
        oldUsers.AddRange(toAppend);
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, oldUsers, _jsonOptions);
        _cache.Clear();
        _cache.AddRange(oldUsers);
        await writeStream.DisposeAsync();
    }
}