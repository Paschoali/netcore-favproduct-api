using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace FavProducts.Data
{
    public class BaseRepository : IDisposable
    {
        #region [ PRIVATE ]

        private readonly object _lockObject = new object();

        private string ConnectionString { get; set; }

        private string ProviderName { get; set; }

        private void OpenConnection(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private IDbConnection CreateDbConnection(string connectionString, string providerName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Error while trying to connect to database.");

            DbConnection connection = null;

            try
            {
                connection = new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Occurred an error while creating the DbProviderFactory for: {providerName}, Error details: {ex}");
            }

            return connection;
        }

        #endregion

        #region [ PROTECTED ]

        protected IDbTransaction Transaction { get; private set; }

        protected IDbConnection Connection { get; private set; }

        #endregion

        #region [ CONSTRUCTORS ]

        public BaseRepository(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }

        #endregion

        public IDbConnection GetConnection()
        {
            lock (_lockObject)
            {
                var connection = CreateDbConnection(ConnectionString, ProviderName);

                OpenConnection(connection);

                return connection;
            }
        }

        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void BeginTransaction(IDbConnection connection)
        {
            Transaction = connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Close(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
    }
}
