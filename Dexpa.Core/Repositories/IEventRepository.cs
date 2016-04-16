using System.Security.Cryptography.X509Certificates;
using Dexpa.Core.Model;

namespace Dexpa.Core.Repositories
{
    public interface IEventRepository : IRepository<SystemEvent>
    {
        SystemEvent Add(SystemEvent item);
    }
}
