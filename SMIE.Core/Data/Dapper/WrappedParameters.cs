using System.Collections.Generic;
using System.Data;
using Dapper;

namespace SMIE.Core.Data.Dapper
{
    /// <summary>
    /// Обертка над динамическими параметрами, позволяющая совмещать различные их типы
    /// </summary>
    public class WrappedParameters : SqlMapper.IDynamicParameters
    {
        private readonly List<SqlMapper.IDynamicParameters> _dynamicParameters;

        /// <summary>
        /// Класс-обертка над динамическими параметрами
        /// </summary>
        public WrappedParameters()
        {
            _dynamicParameters = new List<SqlMapper.IDynamicParameters>();
        }

        /// <summary>
        /// Добавить объект динамических параметров
        /// </summary>
        /// <param name="dynamicParameters"></param>
        public void Add(SqlMapper.IDynamicParameters dynamicParameters)
        {
            _dynamicParameters.Add(dynamicParameters);
        }

        /// <summary>
        /// Для внутреннего пользования
        /// </summary>
        /// <param name="command"></param>
        /// <param name="identity"></param>
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            foreach (var dp in _dynamicParameters)
            {
                dp.AddParameters(command, identity);
            }
        }
    }
}
