using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Infrastructure.Repositories;
using PlayFabBuddy.Lib.Entities.Accounts;
using PlayFabBuddy.Lib.Interfaces.Repositories;

namespace PlayFabBuddy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var repoSettings = new LocalMasterPlayerAccountRepositorySettings(config["defaultSavePath"]);
        services.AddSingleton(repoSettings);

        services.AddTransient<IRepository<MasterPlayerAccountEntity>, LocalMasterPlayerAccountRepository>();

        return services;
    }
}
