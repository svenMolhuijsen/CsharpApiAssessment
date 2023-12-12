using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;
using GeoCoordinatePortable;

namespace API.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;

        public GeolocationService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Testapp");
        }

        /// <summary>
        /// Calculates the distance between two addresses using geocoding.
        /// </summary>
        /// <param name="address1">The first address.</param>
        /// <param name="address2">The second address.</param>
        /// <returns>The distance between the two addresses in kilometers.</returns>
        public async Task<Distance> CalculateDistanceAsync(Address address1, Address address2)
        {
            try
            {
                // Geocode the addresses to GeoCoordinates
                GeoCoordinate coordinates1 = await GeocodeAddressAsync(address1);
                await Task.Delay(1100); // Introduce a delay for rate limiting (Because this is a free API)
                GeoCoordinate coordinates2 = await GeocodeAddressAsync(address2);

                // Calculate distance
                double distanceInKM = coordinates1.GetDistanceTo(coordinates2) / 1000;

                // Create and return the Distance object
                Distance distance = new Distance() { address1 = address1, address2 = address2, distance = distanceInKM };
                return distance;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Geocodes an address using OSM Nominatim API and returns the GeoCoordinate.
        /// </summary>
        /// <param name="address">The address to geocode.</param>
        /// <returns>The GeoCoordinate of the provided address.</returns>
        private async Task<GeoCoordinate> GeocodeAddressAsync(Address address)
        {
            try
            {
                // Geocode the address using OSM Nominatim API
                var geocodingApiUrl = "https://nominatim.openstreetmap.org/search";
                var response = await _httpClient.GetAsync($"{geocodingApiUrl}?format=json&q={Uri.EscapeDataString(address.ToString())}");

                response.EnsureSuccessStatusCode();

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var jsonDocument = await JsonDocument.ParseAsync(responseStream))
                    {

                     
                        // The first result in the response is the best result
                        var result = jsonDocument.RootElement.EnumerateArray().FirstOrDefault();

                        if (result.ValueKind == JsonValueKind.Undefined)
                        {
                            throw new Exception("No result found in the geocoding response.");
                        }

                        // Ensure the conversion from string to double is done correctly System.globalization is used due to different format in response
                        double lat = double.Parse(result.GetProperty("lat").GetString(), System.Globalization.CultureInfo.InvariantCulture);
                        double lon = double.Parse(result.GetProperty("lon").GetString(), System.Globalization.CultureInfo.InvariantCulture);


                        return new GeoCoordinate(lat, lon);
                    }
                }
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
