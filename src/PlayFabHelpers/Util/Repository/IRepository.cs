namespace PlayFabBuddy.PlayFabHelpers.Util.Repository
{
    internal interface IRepository<T>
    {
        public Task Save(List<T> toSave);
        public List<T> Get();
    }
}
