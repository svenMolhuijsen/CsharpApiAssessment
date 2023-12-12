using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AddressRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// Gets a specific address by ID.
        /// </summary>
        public List<Address> GetAdresses()
        {
            return _dbContext.Addresses.ToList();
        }

        /// <summary>
        /// Gets a specific address by ID.
        /// </summary>
        /// <param name="id">The ID of the address to retrieve.</param>
        /// <returns>The address with the specified ID.</returns>        
        public Address GetAddress(int id)
        {
            return _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Gets addresses based on search criteria, sort order, and ascending/descending flag.
        /// </summary>
        /// <param name="searchValue">The search criteria for filtering addresses.</param>
        /// <param name="sortBy">The property to sort addresses by.</param>
        /// <param name="ascending">True if sorting in ascending order, false for descending order.</param>
        /// <returns>The list of addresses based on the provided criteria.</returns>
        public List<Address> GetAddresses(string searchValue, string sortBy, bool ascending)
        {
            // Search logic
            var propertiesOfType = typeof(Address)
                .GetProperties()
                .Where(prop => prop.PropertyType == typeof(string));

            List<Address> addresses;

            if (string.IsNullOrEmpty(searchValue))
            {
                addresses = _dbContext.Addresses.ToList();
            }
            else
            {
                addresses = _dbContext.Addresses
                    .AsEnumerable()
                    .Where(address =>
                        propertiesOfType.Any(prop =>
                            (prop.GetValue(address, null) ?? "").ToString().ToLower().Contains(searchValue.ToLower())))
                    .ToList();
            }

            // Sorting logic
            if (!string.IsNullOrEmpty(sortBy))
            {
                var propertyToSort = typeof(Address)
                    .GetProperties()
                    .FirstOrDefault(prop => prop.Name.Equals(sortBy, StringComparison.OrdinalIgnoreCase));

                if (propertyToSort != null)
                {
                    addresses = ascending
                        ? addresses.OrderBy(a => propertyToSort.GetValue(a, null)).ToList()
                        : addresses.OrderByDescending(a => propertyToSort.GetValue(a, null)).ToList();
                }
            }

            return addresses;
        }

        /// <summary>
        /// Adds a new address to the database.
        /// </summary>
        /// <param name="address">The address to add.</param>
        /// <returns>The added address.</returns>
        public Address AddAddress(Address address)
        {
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
            return address;
        }

        /// <summary>
        /// Updates an existing address by ID.
        /// </summary>
        /// <param name="address">The updated address information.</param>
        /// <param name="id">The ID of the address to update.</param>
        /// <returns>The updated address.</returns>
        public Address UpdateAddress(Address address, int id)
        {
            var result = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (result != null)
            {
                _dbContext.Entry(result).CurrentValues.SetValues(address);
                _dbContext.Entry(result).State = EntityState.Modified;

                _dbContext.SaveChanges();
                return address;
            }
            else
            {
                // Throw an exception if the address to update is not found
                throw new Exception("Could not update the address. Address not found.");
            }
        }

        /// <summary>
        /// Deletes an address by ID.
        /// </summary>
        /// <param name="id">The ID of the address to delete.</param>
        /// <returns>The deleted address.</returns>
        public Address DeleteAddress(int id)
        {
            // Retrieve the address to be deleted to show which address has been removed
            var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (address != null)
            {
                _dbContext.Addresses.Remove(address);
                _dbContext.SaveChanges();
            }
            return address;
        }
    }
}
