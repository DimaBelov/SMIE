using System.Collections.Generic;

namespace SMIE.Core.Data.Dapper
{
    public class TableValuedParameterBase
    {
        public string Name { get; }

        public IEnumerable<object> Collection { get; }

        protected TableValuedParameterBase(string name, IEnumerable<object> collection)
        {
            Name = name;
            Collection = collection;
        }
    }
}
