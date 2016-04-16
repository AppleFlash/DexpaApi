using System;
using Dexpa.Core.Model;

namespace Dexpa.Core.Factories
{
    public class OrderFactory
    {
        public Order CreateOrder(Customer customer, Address fromAddress, Address toAddress, DateTime departureDate)
        {
            return new Order(customer)
            {
                FromAddress = fromAddress,
                ToAddress = toAddress,
                DepartureDate = departureDate
            };
        }
        public Order CreateOrder(Customer customer, Address fromAddress, Address toAddress, DateTime departureDate, Driver driver, double cost)
        {
            return new Order(customer)
            {
                FromAddress = fromAddress,
                ToAddress = toAddress,
                DepartureDate = departureDate,
                Driver = driver,
                Cost = cost,
                State = OrderStateType.Completed
            };
        }
    }
}
