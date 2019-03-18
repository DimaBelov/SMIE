using System.Collections.Generic;
using System.Data;
using Dapper;
using SMIE.Core.Data.Dapper;

namespace SMIE.Core.Data.Specification
{
    public abstract class SpecificationBase : ISpecification
    {
        protected readonly WrappedParameters _wrappedParameters = new WrappedParameters();
        protected readonly DynamicParameters _dynamicParameters = new DynamicParameters();

        public abstract CommandType GetCommandType();
        public abstract string GetCommandText();

        protected SpecificationBase()
        {
            _wrappedParameters.Add(_dynamicParameters);
        }

        protected void AddParameter(string name, object value)
        {
            _dynamicParameters.Add(name, value);
        }

        protected T GetParameter<T>(string name)
        {
            return _dynamicParameters.Get<T>(name);
        }

        public virtual int Output()
        {
            return 0;
        }

        public int? CommandTimeOut { get; protected set; }

        public virtual WrappedParameters GetParameters()
        {
            return _wrappedParameters;
        }
    }

    public class SqlParameters
    {
        public Dictionary<string, string> SingleParameters { get; set; }

        public IList<TableValuedParameterBase> TabledParameters { get; set; }
    }
}
