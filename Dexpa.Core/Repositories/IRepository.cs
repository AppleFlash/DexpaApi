using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Repositories
{
    public interface IRepository<T>
    {
        IList<T> List();

        IList<T> List(System.Linq.Expressions.Expression<Func<T, bool>> expression);

        bool Any(System.Linq.Expressions.Expression<Func<T, bool>> expression);

        void Commit();
    }
}
