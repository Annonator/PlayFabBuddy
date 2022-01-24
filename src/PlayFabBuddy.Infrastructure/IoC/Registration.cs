namespace PlayFabBuddy.Infrastructure.IoC;

public class Registration
{
    public Registration(Func<object> creationDelegate, RegistrationType type)
    {
        Type = type;
        CreationDelegate = creationDelegate;
    }

    public Func<object> CreationDelegate { get; set; }
    public RegistrationType Type { get; set; }
}