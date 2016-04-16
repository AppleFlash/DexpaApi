using Dexpa.Core.Model;

namespace Dexpa.Core.Factories
{
    public class CustomerFactory
    {
        public static Customer CreateCustomer(string name, string phone)
        {
            return new Customer()
            {
                Name = name,
                Phone = phone
            };
        }
    }
}
