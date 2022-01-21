namespace PlayFabBuddy.Lib.Commands
{
    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }
}
