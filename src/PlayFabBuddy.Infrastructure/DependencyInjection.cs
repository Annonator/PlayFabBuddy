using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFab;
using PlayFabBuddy.Infrastructure.Adapter.PlayFab;
using PlayFabBuddy.Infrastructure.Adapter.PlayFab.Admin;
using PlayFabBuddy.Infrastructure.Adapter.PlayFab.Analytics;
using PlayFabBuddy.Infrastructure.Config;
using PlayFabBuddy.Infrastructure.Repositories;
using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var repoSettings = new LocalMasterPlayerAccountRepositorySettings(config["defaultSavePath"]);
        services.AddSingleton(repoSettings);

        services.AddTransient<IRepository<MasterPlayerAccountAggregate>, LocalMasterPlayerAccountRepository>();
        services.AddTransient<LocalMasterPlayerAccountRepository>();

        var segmentDefaultSettings = new SegmentMasterPlayerAccountRepositorySetting();
        services.AddSingleton(segmentDefaultSettings);
        services.AddTransient<SegmentMasterPlayerAccountRepository>();

        var pfConfig = new PlayFabConfig(config["titleId"], config["devSecret"]);
        var adminEntityToken = pfConfig.InitAsync().Result;

        var playFabApiSettings =
            new PlayFabApiSettings { TitleId = config["titleId"], DeveloperSecretKey = config["devSecret"] };

        services.AddSingleton<IConfig>(pfConfig);
        services.AddSingleton(playFabApiSettings);
        services.AddSingleton<PlayFabAdminInstanceAPI>();
        services.AddTransient<IPlayerAccountAdapter, PlayerAccountAdapter>();
        services.AddTransient<IPlayStreamAdapter, PlayStreamAdapter>();

        // Matchmaking
        services.AddTransient<IMatchmakingAdapter, MatchmakingAdapter>();
        services.AddSingleton(new PlayFabAuthenticationContext { EntityToken = adminEntityToken });
        services.AddSingleton<PlayFabMultiplayerInstanceAPI>();

        // Policy
        services.AddTransient<IPolicyAdapter, PolicyAdapter>();

        // Register KUSTO
        var kustoConnectionString = new KustoConnectionStringBuilder(config["PFDataCluster"], config["titleId"])
            .WithAadApplicationKeyAuthentication(config["PFDataClientId"], config["PFDataClientSecret"],
                config["AzureAuthority"]);


        services.AddTransient<ICslQueryProvider>(sp =>
            KustoClientFactory.CreateCslQueryProvider(kustoConnectionString));
        services.AddTransient<IDataExplorerAdapter, DataExplorerAdapter>();

        return services;
    }
}