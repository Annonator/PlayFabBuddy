namespace PlayFabBuddy.Lib.UseCases;

public interface IUseCase<T>
{
    Task<T> ExecuteAsync();
}