using API.Models;

namespace API.Services
{
    public interface IGeolocationService
    {
        Distance calculateDistance(int AddressId1, int AddressId2);
    }
}
