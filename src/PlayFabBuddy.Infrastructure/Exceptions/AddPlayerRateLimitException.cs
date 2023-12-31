namespace PlayFabBuddy.Infrastructure.Exceptions;

public class AddPlayerRateLimitException(string CustomId, uint? retryInSeconds) : Exception
{
    public string CustomId { get; init; } = CustomId;

    public uint? RetryInSeconds { get; init; } = retryInSeconds;
}