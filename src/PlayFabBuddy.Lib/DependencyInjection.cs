using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayFabBuddy.Lib.UseCases.Policy;

namespace PlayFabBuddy.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<AllowCustomLoginUseCase>();
        services.AddTransient<DenyCustomLoginUseCase>();
        services.AddTransient<DenyLinkingCustomIdUseCase>();
        services.AddTransient<AllowLinkingCustomIdUseCase>();
        services.AddTransient<ListAllUseCase>();



        return services;
    }
}
