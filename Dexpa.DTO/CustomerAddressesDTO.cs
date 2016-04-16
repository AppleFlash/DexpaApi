namespace Dexpa.DTO
{
    public class CustomerAddressesDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long CustomerId { get; set; }

        public AddressDTO Address { get; set; }
    }
}