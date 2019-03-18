using System.Data;

namespace SMIE.Core.Data
{
    /// <summary>
    /// Интерфейс менеджера соединений с базой данных
    /// </summary>
    public interface IDbConnectionsManager
    {
        IDbConnection CreateConnection(string serverName);
    }
}
