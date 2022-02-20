namespace PlayFabBuddy.Infrastructure.Exceptions;

public class AddPlayerForbiddenException : Exception
{
    public AddPlayerForbiddenException(string? message) : base("A PlayFab Policy prohibits the creation of a Player with the choosen Identity providder for : " + message)
    {
    }
}
