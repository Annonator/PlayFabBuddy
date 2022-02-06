using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Infrastructure.Adapter.PlayFab;
using PlayFabBuddy.Infrastructure.Adapter.PlayFab.Admin;
using PlayFabBuddy.Infrastructure.Config;
using PlayFabBuddy.Infrastructure.Repositories;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var repoSettings = new LocalMasterPlayerAccountRepositorySettings(config["defaultSavePath"]);
        services.AddSingleton(repoSettings);

        services.AddTransient<IRepository<MasterPlayerAccountEntity>, LocalMasterPlayerAccountRepository>();

        var pfConfig = new PlayFabConfig(config["titleId"], config["devSecret"]);
        pfConfig.InitAsync();

        services.AddSingleton<IConfig>(pfConfig);

        services.AddTransient<IPlayerAccountAdapter, PlayerAccountAdapter>();
        services.AddTransient<IPlayStreamAdapter, PlayStreamAdapter>();

        return services;
    }
}
