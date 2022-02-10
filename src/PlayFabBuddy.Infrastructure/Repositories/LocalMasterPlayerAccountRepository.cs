using System.Text.Json;
using System.Text.Json.Serialization;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure.Repositories;

public class LocalMasterPlayerAccountRepository : IRepository<MasterPlayerAccountAggregate>
{
    private readonly string _configPath;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly List<MasterPlayerAccountAggregate> _cache;
    private DateTime _lastUpdate;

    public LocalMasterPlayerAccountRepository(LocalMasterPlayerAccountRepositorySettings settings)
    {
        _configPath = settings.DefaultSavePath;
        _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        _cache = new List<MasterPlayerAccountAggregate>();
        _lastUpdate = DateTime.MinValue;
    }

    public Task<List<MasterPlayerAccountAggregate>> Get()
    {
        if (!File.Exists(_configPath))
        {
            File.CreateText(_configPath).Close();
            return Task.FromResult(_cache);
        }

        var fileUpdateStamp = File.GetLastWriteTimeUtc(_configPath);

        if (_lastUpdate >= fileUpdateStamp)
        {
            return Task.FromResult(_cache);
        }

        _lastUpdate = fileUpdateStamp;

        var jsonString = File.ReadAllText(_configPath);

        var entityList = JsonSerializer.Deserialize<List<MasterPlayerAccountEntity>>(jsonString, _jsonOptions);

        if (entityList == null)
        {
            throw new NullReferenceException("Entity List, is null, Error on Deserialization");
        }

        var aggregates = new List<MasterPlayerAccountAggregate>();

        foreach (var entity in entityList)
        {
            aggregates.Add(new MasterPlayerAccountAggregate(entity));
        }

        _cache.Clear();
        _cache.AddRange(aggregates);

        return Task.FromResult(_cache);
    }

    public async Task Save(List<MasterPlayerAccountAggregate> toSave)
    {
        var entityList = ConvertToEntity(toSave);
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, entityList, _jsonOptions);
        _cache.Clear();
        _cache.AddRange(toSave);
        await writeStream.DisposeAsync();
    }

    public async Task Append(List<MasterPlayerAccountAggregate> toAppend)
    {
        var entityList = ConvertToEntity(toAppend);
        var oldUsers = Get();
        oldUsers.AddRange(toAppend);
        await using var writeStream = File.Create(_configPath);
        await JsonSerializer.SerializeAsync(writeStream, entityList, _jsonOptions);
        _cache.Clear();
        _cache.AddRange(oldUsers);
        await writeStream.DisposeAsync();
    }

    private List<MasterPlayerAccountEntity> ConvertToEntity(List<MasterPlayerAccountAggregate> aggregates)
    {
        var entityList = new List<MasterPlayerAccountEntity>();
        foreach (var aggregate in aggregates)
        {
            entityList.Add(aggregate.MasterPlayerAccount);
        }
        return entityList;
    }
}