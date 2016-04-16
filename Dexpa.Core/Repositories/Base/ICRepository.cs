namespace Dexpa.Core.Repositories
{
    public interface ICRepository<T> : IRepository<T>
    {
        T Add(T item);
    }
}
