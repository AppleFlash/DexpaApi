using System;
using System.Collections.Generic;

namespace Dexpa.Core.Services
{
    public interface IDataService<T> : IDisposable
    {
        T GetItem(int id);

        T AddItem(T item);

        void DeleteItem(int id);

        IList<T> GetList();

        void UpdateItem(T item);
    }
}
