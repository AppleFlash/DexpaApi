namespace Dexpa.Core.Repositories
{
    public interface ICRUDRepository<T> : ICRURepository<T>
    {
        void Delete(T item);
    }
}
