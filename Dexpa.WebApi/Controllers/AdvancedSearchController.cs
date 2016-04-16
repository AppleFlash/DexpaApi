using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class AdvancedSearchController : ApiControllerBase
    {
        private IAdvancedSearchService mAdvancedSearchService;
        private IGeocoderService mGeocoderService;

        public AdvancedSearchController(IAdvancedSearchService advancedSearchService, IGeocoderService geocoderService)
        {
            mAdvancedSearchService = advancedSearchService;
            mGeocoderService = geocoderService;
        }

        [HttpGet]
        public List<SearchResultDTO> Search(string query)
        {
            List<SearchResultDTO> searchResults = new List<SearchResultDTO>();
            int DispatcherAdvancedSearchResultCount = Convert.ToInt32(WebConfigurationManager.AppSettings["DispatcherAdvancedSearchResultCount"].ToString());

            bool DriversFound = false;
            bool CarsFound = false;
            bool OrdersFound = false;
            bool MapObjectsFound = false;

            while (searchResults.Count < DispatcherAdvancedSearchResultCount)
            {
                if (DriversFound && CarsFound && OrdersFound && MapObjectsFound)
                {
                    break;
                }
                if (!DriversFound)
                {
                    searchResults.AddRange(ObjectMapper.Instance.Map<List<SearchResult>, List<SearchResultDTO>>(mAdvancedSearchService.DriverSearch(query)));
                    DriversFound = true;
                    continue;
                }
                if (!CarsFound)
                {
                    searchResults.AddRange(ObjectMapper.Instance.Map<List<SearchResult>, List<SearchResultDTO>>(mAdvancedSearchService.CarSearch(query)));
                    CarsFound = true;
                    continue;
                }
                if (!OrdersFound)
                {
                    searchResults.AddRange(ObjectMapper.Instance.Map<List<SearchResult>, List<SearchResultDTO>>(mAdvancedSearchService.OrderSearch(query)));
                    OrdersFound = true;
                    continue;
                }
                if (!MapObjectsFound)
                {
                    searchResults.AddRange(ObjectMapper.Instance.Map<List<SearchResult>, List<SearchResultDTO>>(mGeocoderService.ReverseGeocoding(query)));
                    MapObjectsFound = true;
                    continue;
                }
            }

            return searchResults;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mAdvancedSearchService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}