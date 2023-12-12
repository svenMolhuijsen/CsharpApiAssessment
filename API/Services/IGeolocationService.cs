using API.Models;

namespace API.Services
{
    public interface IGeolocationService
    {
        Task<Distance> CalculateDistanceAsync(Address address1, Address address2);
    }
}
