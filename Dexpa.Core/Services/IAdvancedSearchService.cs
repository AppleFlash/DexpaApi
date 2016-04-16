using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IAdvancedSearchService : IDisposable
    {
        List<SearchResult> DriverSearch(string query);

        List<SearchResult> OrderSearch(string query);

        List<SearchResult> CarSearch(string query);
    }
}
