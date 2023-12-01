using API.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using System.Data.Common;

namespace API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Address> GetAdresses()
        {
            using (IDbConnection dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Address>("SELECT * FROM Addresses").AsList();

            }
        }
        public Address GetAddress(int id)
        {
            using (IDbConnection dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();

                //Dapper automatically handles the parameterization of the query, ensuring that user inputs are properly escaped and sanitized against SQL injection.
                return dbConnection.QueryFirstOrDefault<Address>("SELECT * FROM Addresses WHERE Id = @Id", new { Id = id });

            }
        }

        public Address AddAddress(Address address)
        {
            using (IDbConnection dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();
                var query = @"INSERT INTO Addresses (Street, HouseNumber, Postcode, City, Country) 
                      VALUES (@Street, @HouseNumber, @Postcode, @City, @Country);
                      SELECT last_insert_rowid()";

                //Dapper automatically handles the parameterization of the query, ensuring that user inputs are properly escaped and sanitized against SQL injection.
                var newId = dbConnection.QueryFirstOrDefault<int>(query, address);
                address.Id = newId;
                return address;
            }
        }

        public Address DeleteAddress(int id)
        {
            using (IDbConnection dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();

                var query = "DELETE FROM Addresses WHERE Id = @Id";
                var parameters = new { Id = id };

                var deletedAddress = dbConnection.QueryFirstOrDefault<Address>(query, parameters);

                if (deletedAddress != null)
                {
                    return deletedAddress;
                }

                return null; // Address with the given id was not found
            }
        }



        public Address UpdateAddress(Address address)
        {
            using (IDbConnection dbConnection = new SqliteConnection(_connectionString))
            {
                dbConnection.Open();

                var query = @"UPDATE Addresses 
                      SET Street = @Street, 
                          HouseNumber = @HouseNumber, 
                          Postcode = @Postcode, 
                          City = @City, 
                          Country = @Country 
                      WHERE Id = @Id"
                ;

                dbConnection.Execute(query, address);
                return address;
            }
        }

    }
}
