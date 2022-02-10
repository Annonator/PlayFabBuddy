namespace PlayFabBuddy.Lib.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task Save(List<T> toSave);
    public Task Append(List<T> toAppend);
    public Task<List<T>> Get();
}