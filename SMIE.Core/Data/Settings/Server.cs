namespace SMIE.Core.Data.Settings
{
    public class Server
    {
        /// <summary>
        /// Наименование сервера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Строка соединения
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Название enviroment variable со строкой соединения 
        /// </summary>
        public string EnviromentConnString { get; set; }

        /// <summary>
        /// Имя поставщика, если задано
        /// </summary>
        public string ProviderName { get; set; }

        public void AddUserPassword(string userId, string password)
        {
            if (ProviderName.Equals("System.Data.SqlClient"))
                ConnectionString = ConnectionString.Replace("Integrated Security=SSPI;", string.Empty);
            
            if (!ConnectionString.EndsWith(";"))
                ConnectionString += ";";

            ConnectionString += $"User ID={userId};Password={password}";
        }
    }
}
