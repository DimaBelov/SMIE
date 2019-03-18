using System.Collections.Generic;
using System.Data;
using SMIE.Core.Data.Dapper;

namespace SMIE.Core.Data.Specification
{
    public abstract class SqlSpecification : SpecificationBase
    {
        readonly IList<TableValuedParameterBase> _tabledParameters = new List<TableValuedParameterBase>();

        protected void AddParameterIfNotNull(string name, object value)
        {
            if (value != null)
                _dynamicParameters.Add(name, value);
        }

        protected void AddParameter(string name, object value, DbType type, ParameterDirection? direction = null)
        {
            _dynamicParameters.Add(name, value, type, direction);
        }

        protected void AddTableParameter<T>(string name, IEnumerable<T> collection, IList<string> columns = null,
            long maxVarLength = 2000)
        {
            var parameter = new TableValuedParameter<T>(name, collection, columns, maxVarLength);
            _tabledParameters.Add(parameter);
            _wrappedParameters.Add(parameter);
        }

        public override CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        protected abstract string CreateCommandText();

        public override string GetCommandText()
        {
            return CreateCommandText();
        }
    }
}
