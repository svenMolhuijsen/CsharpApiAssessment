using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

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
            try
            {
                List<Address> addresses = _addressRepository.GetAdresses();
                if (addresses.Count == 0)
                {
                    return NoContent(); //204
                }

                return Ok(addresses); //200
            } catch (Exception ex)
            {
                //LogException(ex); not a requirement, not implemented
                return StatusCode(500); //500
            }
        }

        // GET api/Addresses/5
        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {

            try
            {
                var address = _addressRepository.GetAddress(id);

                if (address == null)
                {
                    return NoContent(); //204
                }

                return Ok(address); //200
            }
            catch (Exception ex)
            {
                //LogException(ex); not a requirement, not implemented
                return StatusCode(500); //500
            }
        }



        // GET api/Addresses
        [HttpGet("{searchValue}/{sortBy}/{ascending}")]
        public IActionResult GetAddresses(string searchValue, string sortBy, bool ascending = true )
        {
          
                List<Address> addresses = _addressRepository.GetAddresses(searchValue, sortBy, ascending);

                if (addresses.Count == 0)
                {
                    return NoContent(); // 204
                }

                return Ok(addresses); // 200
         
        }

        [HttpPost]
        public IActionResult AddAddress([FromBody] Address address)
        {
            try
            {
                var newAddress = _addressRepository.AddAddress(address);
                return CreatedAtAction(nameof(GetAddress), new { id = newAddress.Id }, newAddress);
            }
            catch (Exception ex)
            {
                //LogException(ex); not a requirement, not implemented
                return StatusCode(500); //500
            }
        }

        // PUT api/Addresses/5
        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            try
            {
                var existingAddress = _addressRepository.GetAddress(id);
                updatedAddress.Id = id;

                if (existingAddress == null)
                {
                    return NotFound();
                }

                _addressRepository.UpdateAddress(updatedAddress , id);

                return NoContent();
            }
            catch (Exception ex)
            {
                //LogException(ex); not a requirement, not implemented
                return StatusCode(500); //500
            }
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

