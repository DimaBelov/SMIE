using System.Data;

namespace SMIE.Core.Data
{
    public interface IProviderFactory
    {
        IDbConnectionFactory<IDbConnection> GetProvider(string providerName);
    }
}
