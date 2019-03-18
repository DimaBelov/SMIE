using System.Collections.Generic;
using System.Linq;

namespace SMIE.Core.Data.Settings
{
    public class ServersSettings
    {
        /// <summary>
        /// Группы серверов
        /// </summary>
        public List<Server> Servers { get; set; }

        public Server this[string name]
        {
            get { return Servers.FirstOrDefault(s => s.Name.Equals(name)); }
        }
    }
}
