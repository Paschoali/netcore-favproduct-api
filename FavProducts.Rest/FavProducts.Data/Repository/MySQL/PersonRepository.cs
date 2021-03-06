﻿using Dapper;
using FavProducts.Core.Repository;
using FavProducts.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FavProducts.Data.Repository.MySQL
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public PersonRepository(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        #region [ READ ]

        public async Task<IEnumerable<Person>> ListAsync(int pageNumber, int pageSize)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"SELECT Id, Name, Email FROM Person LIMIT @PageSize OFFSET @PageNumber;";

                var personList = await connection.QueryAsync<Person>(sql, new { PageSize = pageSize, PageNumber = (--pageNumber * pageSize) });

                return personList;
            }
        }

        public async Task<Person> GetAsync(Guid personId)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"SELECT Id, Name, Email FROM Person WHERE Id = @PersonId;";

                var person = await connection.QueryAsync<Person>(sql, new { PersonId = personId });

                return person.FirstOrDefault();
            }
        }

        public async Task<bool> GetByEmailAsync(string email)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM Person WHERE Username = @Username;";

                int count = (await connection.QueryAsync<int>(sql, new { Email = email })).FirstOrDefault();

                return count > 0 ? true : false;
            }
        }

        #endregion

        #region [ WRITE ]

        public async Task<Person> CreateAsync(Person person)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"INSERT INTO Person (Id, Name, Username) VALUES (@PersonId, @Name, @Username);";

                await connection.ExecuteAsync(sql, new { PersonId = person.Id, Name = person.Name, Email = person.Email });
            }

            return person;
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"UPDATE Person SET Name = @Name, Username = @Username WHERE Id = @PersonId;";

                await connection.ExecuteAsync(sql, new { PersonId = person.Id, Name = person.Name, Email = person.Email });
            }

            return person;
        }

        public async Task DeleteAsync(Guid personId)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = @"DELETE FROM Person WHERE Id = @PersonId;";

                await connection.ExecuteAsync(sql, new { PersonId = personId });
            }
        }

        #endregion
    }
}