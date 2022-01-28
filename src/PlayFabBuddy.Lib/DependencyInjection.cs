using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Lib.Admin;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Util.Config;
using PlayFabBuddy.Lib.Util.Repository;

namespace PlayFabBuddy.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, IConfiguration config)
    {
        var repoSettings = new LocalMasterPlayerAccountRepositorySettings(config["defaultSavePath"]);
        services.AddSingleton(repoSettings);

        services.AddTransient<IRepository<MasterPlayerAccountEntity>, LocalMasterPlayerAccountRepository>();


        var pfConfig = new PlayFabConfig(config["titleId"], config["devSecret"]);
        pfConfig.InitAsync();

        services.AddSingleton<IConfig>(pfConfig);

        return services;
    }
}
