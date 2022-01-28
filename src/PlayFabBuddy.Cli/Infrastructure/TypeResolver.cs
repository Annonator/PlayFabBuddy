using Spectre.Console.Cli;

namespace PlayFabBuddy.Cli.Infrastructure;

public class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public TypeResolver(IServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    public void Dispose()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        return _serviceProvider.GetService(type);
    }
}
