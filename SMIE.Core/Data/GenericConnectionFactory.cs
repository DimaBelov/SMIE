using System.Data;

namespace SMIE.Core.Data
{
    public class GenericConnectionFactory<T> : IDbConnectionFactory<T>
        where T : IDbConnection, new()
    {
        private static volatile IDbConnectionFactory<T> _instance;
        private static readonly object _lock = new object();

        private GenericConnectionFactory()
        {
        }

        public static IDbConnectionFactory<T> GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GenericConnectionFactory<T>();
                        }
                    }
                }

                return _instance;
            }
        }

        public T CreateConnection(string connectionString)
        {
            return new T {ConnectionString = connectionString};
        }
    }
}
