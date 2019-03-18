using System.Data;

namespace SMIE.Core.Data
{
    public interface IDbConnectionFactory<out T> where T : IDbConnection
    {
        T CreateConnection(string connectionString);
    }
}
