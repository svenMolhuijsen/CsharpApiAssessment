using API.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AddressRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Address> GetAdresses()
        {
            return _dbContext.Addresses.ToList();
        }

        public Address GetAddress(int id)
        {
            return _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
        }

        public List<Address> GetAddresses(string searchValue, string sortBy, bool ascending)
        {

            // Search logic
            var propertiesOfType = typeof(Address)
               .GetProperties()
               .Where(prop => prop.PropertyType == typeof(string));

            List<Address> addresses = null;


            if (string.IsNullOrEmpty(searchValue)) {
                addresses = _dbContext.Addresses.ToList();}
            else {
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

        public Address AddAddress(Address address)
        {
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
            return address;
        }

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
                throw new Exception("couldntUpdateException");
            }

        }

        public Address DeleteAddress(int id)
        {
            //chose to get theb address so its shown which address has been removed
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
