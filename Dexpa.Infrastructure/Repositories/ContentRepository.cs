using System.Data.Entity;
using Dexpa.Core.Model;

namespace Dexpa.Infrastructure.Repositories
{
    public class ContentRepository : ARepository<Content>, IContentRepository
    {
        public ContentRepository(DbContext context)
            : base(context)
        {
        }
    }
}
