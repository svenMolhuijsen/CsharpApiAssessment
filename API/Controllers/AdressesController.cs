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
        /// Gets all addresses.
        /// </summary>
        /// <returns>Returns a list of all addresses.</returns>
        [HttpGet]
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
                // Log and handle exceptions (not implemented in this example)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Gets an address by ID.
        /// </summary>
        /// <param name="id">The ID of the address.</param>
        /// <returns>Returns the address with the specified ID.</returns>
        [HttpGet("{id}")]
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
                // Log and handle exceptions (not implemented in this example)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Gets filtered and sorted addresses.
        /// </summary>
        /// <param name="searchValue">The search value for filtering.</param>
        /// <param name="sortBy">The property name for sorting.</param>
        /// <param name="ascending">Indicates whether to sort in ascending order (default is true).</param>
        /// <returns>Returns filtered and sorted addresses.</returns>
        [HttpGet("{searchValue}/{sortBy}/{ascending}")]
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
                // Log and handle exceptions (not implemented in this example)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Adds a new address.
        /// </summary>
        /// <param name="address">The address to be added.</param>
        /// <returns>Returns the newly added address.</returns>
        [HttpPost]
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
                // Log and handle exceptions (not implemented in this example)
                // Return 500 - Internal Server Error for unhandled exceptions
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="id">The ID of the address to be updated.</param>
        /// <param name="updatedAddress">The updated address information.</param>
        /// <returns>Returns No Content if the update is successful.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            try
            {
                // Retrieve the existing address by ID from the repository
                var existingAddress = _addressRepository.GetAddress(id);

                if (existingAddress == null)
                {
                    // Address not found, return 404 - Not Found
                    return NotFound();
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
        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            // Retrieve the address to be deleted by ID from the repository
            var address = _addressRepository.GetAddress(id);

            if (address ==
null)
            {
                // Address not found, return 404 - Not Found
                return NotFound();
            }

            // Delete the address from the repository
            _addressRepository.DeleteAddress(id);

            // Return 204 - No Content
            return NoContent();
        }

        /// <summary>
        /// Calculates the distance between two addresses.
        /// </summary>
        /// <param name="addressId1">The ID of the first address.</param>
        /// <param name="addressId2">The ID of the second address.</param>
        /// <returns>Returns the calculated distance between the two addresses.</returns>
        [HttpGet("calculateDistance/{addressId1}/{addressId2}")]
        public async Task<IActionResult> CalculateDistanceAsync(int addressId1, int addressId2)
        {
            // Retrieve addresses by ID from the repository
            var address1 = _addressRepository.GetAddress(addressId1);
            var address2 = _addressRepository.GetAddress(addressId2);

            if (address1 == null || address2 == null)
            {
                // One or both addresses not found, return 404 - Not Found
                return NotFound();
            }

            // Calculate the distance between the two addresses asynchronously
            var distance = await _geolocationService.CalculateDistanceAsync(address1, address2);

            // Return 200 - OK with the calculated distance
            return Ok(distance);
        }
    }
}

