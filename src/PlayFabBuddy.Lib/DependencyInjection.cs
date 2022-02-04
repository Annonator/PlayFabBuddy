using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlayFabBuddy.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, IConfiguration config)
    {


        return services;
    }
}
