namespace PlayFabBuddy.PlayFabHelpers.Commands
{
    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }
}
