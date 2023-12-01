using API.Models;

namespace API.Repositories
{
    public interface IAddressRepository
    {
        List<Address> GetAdresses();
        Address GetAddress(int id);
        Address AddAddress(Address address);
        Address UpdateAddress(Address address);
        Address DeleteAddress(int id);
    }
}
