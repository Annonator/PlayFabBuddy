namespace PlayFabBuddy.PlayFabHelpers.Util.Repository
{
    public interface IRepository<T>
    {
        public Task Save(List<T> toSave);
        public List<T> Get();
    }
}
