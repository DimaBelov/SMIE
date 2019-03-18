using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace SMIE.Core.Data
{
    public class ProviderFactory : IProviderFactory
    {
        public IDbConnectionFactory<IDbConnection> GetProvider(string providerName)
        {
            switch (providerName)
            {
                case "Npgsql":
                    return GenericConnectionFactory<NpgsqlConnection>.GetInstance;
                case "System.Data.SqlClient":
                    return GenericConnectionFactory<SqlConnection>.GetInstance;
                default:
                    return null;
            }
        }
    }
}
