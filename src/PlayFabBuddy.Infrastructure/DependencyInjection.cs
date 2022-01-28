using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Lib.Admin;
using PlayFabBuddy.Lib.Util.Config;

namespace PlayFabBuddy.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }
}
