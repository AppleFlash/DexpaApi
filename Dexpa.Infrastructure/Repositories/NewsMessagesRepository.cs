using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class NewsMessagesRepository : ARepository<NewsMessage>, INewsMessagesRepository
    {
        public NewsMessagesRepository(DbContext context)
            : base(context)
        {
        }
    }
}
