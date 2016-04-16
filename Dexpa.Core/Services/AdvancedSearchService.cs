using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class AdvancedSearchService : IAdvancedSearchService
    {

        private IDriverRepository mDriverRepository;

        private IOrderRepository mOrderRepository;

        private ICarRepository mCarRepository;

        public AdvancedSearchService(IDriverRepository driverRepository, IOrderRepository orderRepository,
            ICarRepository carRepository)
        {
            mDriverRepository = driverRepository;
            mOrderRepository = orderRepository;
            mCarRepository = carRepository;
        }

        public List<SearchResult> DriverSearch(string query)
        {
            query = query.ToLower();

            List<SearchResult> searchResults = new List<SearchResult>();

            List<Driver> drivers = mDriverRepository.List().Where(d => (d.Car != null ? d.Car.Callsign.ToLower().StartsWith(query) : false) ||
                                                                       (d.LastName != null ? d.LastName.ToLower().StartsWith(query) : false) ||
                                                                       (d.FirstName != null ? d.FirstName.ToLower().StartsWith(query) : false) ||
                                                                       (d.MiddleName != null ? d.MiddleName.ToLower().StartsWith(query) : false)).ToList();

            for (int i = 0; i < drivers.Count; i++)
            {
                SearchResult searchResult = new SearchResult();
                searchResult.Driver = drivers[i];
                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        public List<SearchResult> OrderSearch(string query)
        {
            query = query.ToLower();
            List<SearchResult> searchResults = new List<SearchResult>();

            DateTime nowDate = DateTime.UtcNow;

            List<Order> orders =
                mOrderRepository.List(o => o.DepartureDate >= nowDate)
                    .Where(
                        o => o.Id.ToString().ToLower().StartsWith(query) ||
                             (o.Driver != null ? o.Driver.LastName.ToLower().StartsWith(query) : false) ||
                             (o.Driver != null ? o.Driver.FirstName.ToLower().StartsWith(query) : false) ||
                             (o.Driver != null ? o.Driver.MiddleName.ToLower().StartsWith(query) : false) ||
                             (o.Customer != null ? o.Customer.Name != null ? o.Customer.Name.ToLower().StartsWith(query) : false : false) ||
                             (o.Customer != null ? o.Customer.Phone != null ? o.Customer.Phone.ToString().ToLower().StartsWith(query) : false : false) ||
                             o.FromAddress != null ? o.FromAddress.FullName.ToLower().StartsWith(query) : false ||
                             o.ToAddress != null ? o.ToAddress.FullName.ToLower().StartsWith(query) : false)
                    .ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                SearchResult searchResult = new SearchResult();
                searchResult.Order = orders[i];
                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        public List<SearchResult> CarSearch(string query)
        {
            query = query.ToLower();

            List<SearchResult> searchResults = new List<SearchResult>();

            List<Car> cars = mCarRepository.List().Where(c => c.Brand.ToLower().StartsWith(query) || c.Model.ToLower().StartsWith(query) || c.Callsign.ToLower().StartsWith(query)).ToList();

            for (int i = 0; i < cars.Count; i++)
            {
                SearchResult searchResult = new SearchResult();
                searchResult.Car = cars[i];
                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        public void Dispose()
        {
            mCarRepository.Dispose();
            mOrderRepository.Dispose();
            mDriverRepository.Dispose();
        }
    }
}
