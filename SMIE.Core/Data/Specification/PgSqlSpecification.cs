using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace SMIE.Core.Data.Specification
{
    public abstract class PgSqlSpecification : SpecificationBase
    {
        protected void AddParameter(string name, object value, DbType? type = null)
        {
            _dynamicParameters.Add(name, value, type);
        }

        protected void AddParameterIfNotNull(string name, object value, DbType? type = null)
        {
            if(value != null)
                _dynamicParameters.Add(name, value, type);
        }

        protected void AddCompositeTable<T>(string name, IEnumerable<T> value, string pgTypeName) where T: new()
        {
            NpgsqlConnection.MapCompositeGlobally<T>(pgTypeName);
            _dynamicParameters.Add(name, value);
        }

        protected abstract string CreateCommandText();

        public override string GetCommandText()
        {
            return CreateCommandText();
        }

        public override CommandType GetCommandType()
        {
            return CommandType.Text;
        }
    }
}
