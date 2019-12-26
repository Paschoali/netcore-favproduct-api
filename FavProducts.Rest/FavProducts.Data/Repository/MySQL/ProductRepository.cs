using Dapper;
using FavProducts.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FavProducts.Data.Repository.MySQL
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        #region [ READ ]

        public async Task<bool> GetAsync(Guid productId, Guid personId)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM ProductPerson WHERE PersonId = @PersonId AND ProductId = @ProductId;";

                int count = (await connection.QueryAsync<int>(sql, new { PersonId = personId, ProductId = productId })).FirstOrDefault();

                return count > 0 ? true : false;
            }
        }

        public async Task<IEnumerable<Guid>> ListPersonProductIdsAsync(Guid personId, int pageNumber, int pageSize)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = "SELECT ProductId FROM ProductPerson WHERE PersonId = @PersonId LIMIT @PageSize OFFSET @PageNumber;";

                return await connection.QueryAsync<Guid>(sql, new { PersonId = personId, PageSize = pageSize, PageNumber = (--pageNumber * pageSize) });
            }
        }

        #endregion

        #region [ WRITE ]

        public async Task AddAsync(Guid productId, Guid personId)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = "INSERT INTO ProductPerson (PersonId, ProductId) VALUES (@PersonId, @ProductId);";

                await connection.ExecuteAsync(sql, new { PersonId = personId, ProductId = productId });
            }
        }

        public async Task RemoveFromPersonListAsync(Guid productId, Guid personId)
        {
            using (IDbConnection connection = GetConnection())
            {
                string sql = "DELETE FROM ProductPerson WHERE PersonId = @PersonId AND ProductId = @ProductId;";

                await connection.ExecuteAsync(sql, new { PersonId = personId, ProductId = productId });
            }
        }

        #endregion
    }
}