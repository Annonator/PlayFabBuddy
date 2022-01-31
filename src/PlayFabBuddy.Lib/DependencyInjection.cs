using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Lib.Admin;
using PlayFabBuddy.Lib.Util.Config;

namespace PlayFabBuddy.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, IConfiguration config)
    {
        var pfConfig = new PlayFabConfig(config["titleId"], config["devSecret"]);
        pfConfig.InitAsync();

        services.AddSingleton<IConfig>(pfConfig);

        return services;
    }
}
