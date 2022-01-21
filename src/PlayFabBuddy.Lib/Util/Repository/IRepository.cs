namespace PlayFabBuddy.Lib.Util.Repository
{
    public interface IRepository<T>
    {
        public Task Save(List<T> toSave);
        public List<T> Get();
    }
}
