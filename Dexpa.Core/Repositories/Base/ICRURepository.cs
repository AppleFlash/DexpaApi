
namespace Dexpa.Core.Repositories
{
    public interface ICRURepository<T> : IRepository<T>, ICRepository<T>
    {
        T Update(T item);
    }
}
