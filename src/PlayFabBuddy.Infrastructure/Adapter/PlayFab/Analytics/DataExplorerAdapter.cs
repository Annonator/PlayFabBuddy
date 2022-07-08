using System.Net;
using System.Text.Json;
using Kusto.Data.Common;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;

namespace PlayFabBuddy.Infrastructure.Adapter.PlayFab.Analytics;

public class DataExplorerAdapter : IDataExplorerAdapter
{
    private readonly ICslQueryProvider _kustoQueryProvider;

    public DataExplorerAdapter(ICslQueryProvider kustoQueryProvider)
    {
        _kustoQueryProvider = kustoQueryProvider;
    }

    public Task<List<MasterPlayerAccountEntity>> GetPlayersByIp(IPAddress ip)
    {
        var query = "['events.all'] " +
                    "| where FullName_Name == 'player_logged_in' " +
                    "| where EventData.IPV4Address == '" + ip + "'";
        var clientRequestProperties = new ClientRequestProperties {
            Application = "PlayFabBuddy", ClientRequestId = Guid.NewGuid().ToString()
        };

        var entityList = new List<MasterPlayerAccountEntity>();

        using (var reader = _kustoQueryProvider.ExecuteQuery(query, clientRequestProperties))
        {
            while (reader.Read())
            {
                var rawObjectData = reader.GetValue(6);

                var eventData = JsonSerializer.Deserialize<EventData>(rawObjectData.ToString() ?? string.Empty);

                if (eventData != null)
                {
                    entityList.Add(new MasterPlayerAccountEntity {
                        Id = eventData.EntityId, LastKnownIp = eventData.IPV4Address
                    });
                }
            }
        }

        return Task.FromResult(entityList);
    }

    private class EventData
    {
        public PlayFabEnvironment? PlayFabEnvironment { get; set; }
        public string? EventNamespace { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? EntityType { get; set; }
        public string? SourceType { get; set; }
        public string? EventName { get; set; }
        public string? EntityId { get; set; }
        public string? EventId { get; set; }
        public string? TitleId { get; set; }
        public string? Source { get; set; }
        public string? PlatformUserId { get; set; }
        public List<object>? ExperimentVariants { get; set; }
        public string? Platform { get; set; }
        public string? IPV4Address { get; set; }
        public Location? Location { get; set; }
    }

    private class Location
    {
        public string? ContinentCode { get; set; }
        public string? CountryCode { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? City { get; set; }
    }
    private class PlayFabEnvironment
    {
        public string? Application { get; set; }
        public string? Vertical { get; set; }
        public string? Commit { get; set; }
        public string? Cloud { get; set; }
    }
}