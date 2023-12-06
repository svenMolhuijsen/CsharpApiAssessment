using API.Models;

namespace API.Repositories
{
    public interface IAddressRepository
    {
        List<Address> GetAdresses();
        Address GetAddress(int id);
        List<Address> GetAddresses(string searchValue, string sortBy, bool ascending);
        Address AddAddress(Address address);
        Address UpdateAddress(Address address, int id);
        Address DeleteAddress(int id);
    }
}
