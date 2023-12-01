using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IGeolocationService _geolocationService;

        public AdressesController(IAddressRepository addressRepository, IGeolocationService geolocationService)
        {
            _addressRepository = addressRepository;
            _geolocationService = geolocationService;
        }

        // GET: api/Addresses
        [HttpGet]
        public IActionResult GetAddresses()
        {
            List<Address> addresses = _addressRepository.GetAdresses();
            return Ok(addresses);
        }

        // GET api/Addresses/5
        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = _addressRepository.GetAddress(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        // POST api/Addresses
        [HttpPost]
        public IActionResult AddAddress([FromBody] Address address)
        {
            var newAddress = _addressRepository.AddAddress(address);
            return CreatedAtAction(nameof(GetAddress), new { id = newAddress.Id }, newAddress);
        }

        // PUT api/Addresses/5
        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            var existingAddress = _addressRepository.GetAddress(id);

            if (existingAddress == null)
            {
                return NotFound();
            }

            _addressRepository.UpdateAddress(updatedAddress);

            return NoContent();
        }

        // DELETE api/Addresses/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = _addressRepository.GetAddress(id);

            if (address == null)
            {
                return NotFound();
            }

            _addressRepository.DeleteAddress(id);

            return NoContent();
        }

        // Example of using the geolocation service
        [HttpGet("calculateDistance/{addressId1}/{addressId2}")]
        public IActionResult CalculateDistance(int addressId1, int addressId2)
        {
            Distance distance = _geolocationService.calculateDistance(addressId1, addressId2);
            return Ok(distance);
        }
    }
}

