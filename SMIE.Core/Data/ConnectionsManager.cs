using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Options;
using SMIE.Core.Data.Settings;

namespace SMIE.Core.Data
{
    public class ConnectionsManager : IDbConnectionsManager
    {
        readonly IProviderFactory _providerFactory;
        protected readonly List<Server> Servers;

        public ConnectionsManager(IProviderFactory providerFactory, IOptions<ServersSettings> balancedServerOptions)
        {
            _providerFactory = providerFactory;
            Servers = balancedServerOptions.Value.Servers;
        }

        public IDbConnection CreateConnection(string serverName)
        {
            var server = Servers.Find(s => s.Name == serverName);
            if (server == null)
                throw new Exception($"Не найден сервер для имени {serverName}");

            var provider = _providerFactory.GetProvider(server.ProviderName);
            if (provider == null)
                throw new Exception($"Нет реализации создания подключения для провайдера {server.ProviderName}.");

            var conn = provider.CreateConnection(server.ConnectionString);
            if (conn == null)
                throw new Exception("Невозможно создать подключение.");

            return conn;
        }      
    }
}
