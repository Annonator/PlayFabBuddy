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
        if (config["defaultSavePath"] is not null)
        {
            var repoSettings = new LocalMasterPlayerAccountRepositorySettings(config["defaultSavePath"]!);
            services.AddSingleton(repoSettings);
        }
        else
        {
            throw new ArgumentNullException("We are missing the defaultSavePath config variable");
        }

        services.AddTransient<IRepository<MasterPlayerAccountAggregate>, LocalMasterPlayerAccountRepository>();
        services.AddTransient<LocalMasterPlayerAccountRepository>();

        var segmentDefaultSettings = new SegmentMasterPlayerAccountRepositorySetting();
        services.AddSingleton(segmentDefaultSettings);
        services.AddTransient<SegmentMasterPlayerAccountRepository>();

        string adminEntityToken;
        PlayFabConfig pfConfig;

        if (config["titleId"] is not null && config["devSecret"] is not null)
        {
            pfConfig = new PlayFabConfig(config["titleId"]!, config["devSecret"]!);
            adminEntityToken = pfConfig.InitAsync().Result;
        }
        else
        {
            throw new ArgumentNullException("We are missing the titleId or devSecret config variable");
        }

        var playFabApiSettings =
            new PlayFabApiSettings { TitleId = config["titleId"], DeveloperSecretKey = config["devSecret"] };

        services.AddSingleton(playFabApiSettings);
        services.AddSingleton<IConfig>(pfConfig);
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
        if (config["PFDataCluster"] is not null || config["PFDataClientId"] is not null ||
            config["PFDataClientSecret"] is not null)
        {
            var kustoConnectionString = new KustoConnectionStringBuilder(config["PFDataCluster"], config["titleId"])
                .WithAadApplicationKeyAuthentication(config["PFDataClientId"], config["PFDataClientSecret"],
                    config["AzureAuthority"]);

            services.AddTransient<ICslQueryProvider>(sp =>
                KustoClientFactory.CreateCslQueryProvider(kustoConnectionString));
            services.AddTransient<IDataExplorerAdapter, DataExplorerAdapter>();
        }


        return services;
    }
}