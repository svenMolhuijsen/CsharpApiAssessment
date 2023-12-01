using API.Models;

namespace API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Address AddAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public Address DeleteAddress(int id)
        {
            throw new NotImplementedException();
        }

        public Address GetAddress(int id)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAdresses()
        {
            throw new NotImplementedException();
        }

        public Address UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
