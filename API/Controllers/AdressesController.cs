using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    /// <summary>
    /// API controller for managing addresses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IGeolocationService _geolocationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressesController"/> class.
        /// </summary>
        /// <param name="addressRepository">The address repository.</param>
        /// <param name="geolocationService">The geolocation service.</param>
        public AddressesController(IAddressRepository addressRepository, IGeolocationService geolocationService)
        {
            _addressRepository = addressRepository;
            _geolocationService = geolocationService;
        }

        /// <summary>
        /// Retrieves all addresses.
        /// </summary>
        /// <returns>Returns a list of all addresses.</returns>
        /// <response code="200">Returns a list of all addresses.</response>
        /// <response code="204">No addresses found.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Address>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult GetAddresses()
        {
            try
            {
                // Retrieve all addresses from the repository
                var addresses = _addressRepository.GetAdresses();

                if (addresses.Count == 0)
                {
                    // No addresses found, return 204 - No Content
                    return NoContent();
                }

                // Return 200 - OK with the list of addresses
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions (not implemented)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves an address by ID.
        /// </summary>
        /// <param name="id">The ID of the address.</param>
        /// <returns>Returns the address with the specified ID.</returns>
        /// <response code="200">Returns the address with the specified ID.</response>
        /// <response code="204">No address found.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Address), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult GetAddress(int id)
        {
            try
            {
                // Retrieve address by ID from the repository
                var address = _addressRepository.GetAddress(id);

                if (address == null)
                {
                    // Address not found, return 204 - No Content
                    return NoContent();
                }

                // Return 200 - OK with the requested address
                return Ok(address);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions (not implemented)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves filtered and sorted addresses.
        /// </summary>
        /// <param name="searchValue">The search value for filtering.</param>
        /// <param name="sortBy">The property name for sorting.</param>
        /// <param name="ascending">Indicates whether to sort in ascending order (default is true).</param>
        /// <returns>Returns filtered and sorted addresses.</returns>
        /// <response code="200">Returns filtered and sorted addresses.</response>
        /// <response code="204">No addresses found.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{searchValue}/{sortBy}/{ascending}")]
        [ProducesResponseType(typeof(List<Address>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult GetAddresses(string searchValue, string sortBy, bool ascending = true)
        {
            try
            {
                // Retrieve filtered and sorted addresses from the repository
                var addresses = _addressRepository.GetAddresses(searchValue, sortBy, ascending);

                if (addresses.Count == 0)
                {
                    // No addresses found, return 204 - No Content
                    return NoContent();
                }

                // Return 200 - OK with the filtered and sorted addresses
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Adds a new address.
        /// </summary>
        /// <param name="address">The address to be added.</param>
        /// <returns>Returns the newly added address.</returns>
        /// <response code="201">Returns the newly added address.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Address), 201)]
        [ProducesResponseType(500)]
        public IActionResult AddAddress([FromBody] Address address)
        {
            try
            {
                // Add a new address to the repository
                var newAddress = _addressRepository.AddAddress(address);

                // Return 201 - Created with the new address in the response body
                return CreatedAtAction(nameof(GetAddress), new { id = newAddress.Id }, newAddress);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions (not implemented)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="id">The ID of the address to be updated.</param>
        /// <param name="updatedAddress">The updated address information. the ID in model is ignored</param>
        /// <returns>Returns No Content if the update is successful.</returns>
        /// <response code="204">No Content</response>
        /// <response code="404">Existing address Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// 
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            try
            {
                // Retrieve the existing address by ID from the repository
                var existingAddress = _addressRepository.GetAddress(id);

                if (existingAddress == null)
                {
                    // Address not found, return 404 - Not Found
                    return NotFound("Address not found");
                }

                // Set the ID of the updated address and update it in the repository
                updatedAddress.Id = id;
                _addressRepository.UpdateAddress(updatedAddress, id);

                // Return 204 - No Content
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log and handle exceptions (not implemented in this example)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Deletes an address by ID.
        /// </summary>
        /// <param name="id">The ID of the address to be deleted.</param>
        /// <returns>Returns No Content if the deletion is successful.</returns>
        /// <response code="204">No Content</response>
        /// <response code="404">Address Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteAddress(int id)
        {
            try
            {
                // Retrieve the address to be deleted
                var address = _addressRepository.GetAddress(id);

                if (address == null)
                {
                    return NotFound("Address not found");
                }

                _addressRepository.DeleteAddress(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log and handle exceptions (not implemented)
                return StatusCode(500);
            }
        }

            /// <summary>
            /// Calculates the distance between two addresses.
            /// </summary>
            /// <param name="addressId1">The ID of the first address.</param>
            /// <param name="addressId2">The ID of the second address.</param>
            /// <returns>Returns the calculated distance between the two addresses.</returns>
            /// <response code="200">Returns the calculated distance between the two addresses.</response>
            /// <response code="404">One of the adresses is n0t fond</response>
            /// <response code="500">Internal Server Error</response>
            [HttpGet("calculateDistance/{addressId1}/{addressId2}")]
        [ProducesResponseType(typeof(Distance), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        
            public async Task<IActionResult> CalculateDistanceAsync(int addressId1, int addressId2)
        {
            try
            {
                // Retrieve addresses by ID from the repository
                var address1 = _addressRepository.GetAddress(addressId1);
                var address2 = _addressRepository.GetAddress(addressId2);

                if (address1 == null || address2 == null)
                {
                    return NotFound("One or both addresses not found in DB");
                }

                // Calculate the distance between the two addresses asynchronously
                var distance = await _geolocationService.CalculateDistanceAsync(address1, address2);

                // Return 200 - OK with the calculated distance
                return Ok(distance);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No result found in the geocoding response.")
                {
                    // Return 404 - Not Found when geocoding response has no result
                    return NotFound("No result found in the geocoding response.");
                }

                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }
    }
}

