using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class EventRepository : ARepository<SystemEvent>, IEventRepository
    {
        public EventRepository(DbContext context)
            : base(context)
        {
        }
    }
}
