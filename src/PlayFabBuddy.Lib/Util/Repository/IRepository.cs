namespace PlayFabBuddy.Lib.Util.Repository;

public interface IRepository<T>
{
    public Task Save(List<T> toSave);
    public Task Append(List<T> toAppend);
    public List<T> Get();
}