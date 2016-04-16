using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IGeocoderService
    {
        List<SearchResult> ReverseGeocoding(string query);

        GeoPoint Geocoding(string address);
    }
}
