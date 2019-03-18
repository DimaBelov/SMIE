using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.SqlServer.Server;

namespace SMIE.Core.Data.Dapper
{
    /// <summary>
    /// Табличный тип данных с одним или несколькими столбцами произвольного типа
    /// </summary>
    public sealed class TableValuedParameter<T> : TableValuedParameterBase, SqlMapper.IDynamicParameters
    {
        // ReSharper disable StaticFieldInGenericType
        private static readonly SqlDbType SqlDbType;
        private static readonly bool IsObject;
        // ReSharper restore StaticFieldInGenericType

        private readonly long _maxVarLength;
        private IEnumerable<T> TypedCollection => Collection.Cast<T>();
        private readonly IList<string> _columns;
        private readonly Dictionary<string, SqlDbType> _sqlDbTypes;
        private readonly Dictionary<string, PropertyInfo> _properties;

        /// <summary>
        /// Создает параметр табличного типа данных для хранимой процедуры
        /// <para>Позволяет передавать коллекцию значимых типов или объектов</para>
        /// </summary>
        /// <param name="name">Имя переменной в хранимой процедуре</param>
        /// <param name="collection">Коллекция значений для переменной</param>
        /// <param name="columns">Список строк, задающий нужные поля объекта и их порядок</param>
        /// <param name="maxVarLength">Максимальная длина для переменного типа. Если необходимо передать строку длиной больше 4к символов то в параметр передать SqlMetaData.Max</param>
        public TableValuedParameter(string name, IEnumerable<T> collection, IList<string> columns = null, long maxVarLength = 2000)
            : base(name, collection.Cast<object>().ToArray())
        {
            _maxVarLength = maxVarLength;
            _properties = GetTypeProperties().ToDictionary(property => property.Name);
            if (IsObject)
            {
                _columns = columns ?? _properties.Keys.ToList();
                _sqlDbTypes = LookupDbTypes();
            }
        }

        /// <summary>
        /// Для внутреннего пользования
        /// </summary>
        /// <param name="command"></param>
        /// <param name="identity"></param>
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            // Create an SqlMetaData object that describes our table type.

            var tvpDefinition = GetSqlMetaData();
            // Add the table parameter.
            var p = sqlCommand.Parameters.Add(Name, SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;

            if (TypedCollection == null || !TypedCollection.Any())
            {
                p.Value = null;
            }
            else
            {
                // Create a new record, using the metadata array above. Add it to the list.
                if (!IsObject)
                {
                    var list = TypedCollection.Select(n => GetSqlDataRecord(new SqlDataRecord(tvpDefinition), n));
                    p.Value = list;
                }
                else
                {
                    var list = new List<SqlDataRecord>();
                    foreach (var obj in TypedCollection)
                    {
                        var sqlDataRecord = new SqlDataRecord(tvpDefinition);
                        for (var i = 0; i < _columns.Count; i++)
                        {
                            var value = _properties[_columns[i]].GetValue(obj);
                            sqlDataRecord = GetSqlDataRecord(sqlDataRecord, value, i);
                        }
                        list.Add(sqlDataRecord);
                    }
                    p.Value = list;
                }
            }
        }

        #region Приватные методы

        //Получаем метаданные
        private SqlMetaData[] GetSqlMetaData()
        {
            SqlMetaData[] meta;
            if (!IsObject)
            {
                meta = new SqlMetaData[1];
                if (typeof(T) == typeof(string) || typeof(T) == typeof(char) || typeof(T) == typeof(char?))
                    meta[0] = new SqlMetaData("n", SqlDbType, _maxVarLength);
                else
                    meta[0] = new SqlMetaData("n", SqlDbType);
                return meta;
            }
            meta = new SqlMetaData[_sqlDbTypes.Count];
            int i = 0;
            foreach (var pair in _sqlDbTypes)
            {
                switch (pair.Value)
                {
                    case (SqlDbType.Decimal):
                        {
                            meta[i] = new SqlMetaData(pair.Key, pair.Value, 24, 12);
                            break;
                        }
                    case (SqlDbType.NVarChar):
                        {
                            meta[i] = new SqlMetaData(pair.Key, pair.Value, _maxVarLength);
                            break;
                        }
                    case (SqlDbType.VarBinary):
                        {
                            meta[i] = new SqlMetaData(pair.Key, pair.Value, 8000);
                            break;
                        }
                    default:
                        {
                            meta[i] = new SqlMetaData(pair.Key, pair.Value);
                            break;
                        }
                }

              
                i++;
            }
            return meta;
        }

        //Заполняем строку данных
        private SqlDataRecord GetSqlDataRecord<TT>(SqlDataRecord rec, TT value, int pos = 0)
        {
            if (value == null)
            {
                rec.SetDBNull(pos);
                return rec;
            }

            var type = UnfoldType(value.GetType());

            if (type == typeof(byte[]))
            {
                var buffer = (byte[])Convert.ChangeType(value, type);
                rec.SetBytes(pos, 0, buffer, 0, buffer.Length);
            }
            else if (TableValuedParameterTypeMap.TypeMap.ContainsKey(type))
            {
                rec.SetValue(pos, value);
            }
            else
            {
                throw new NotSupportedException(
                    string.Format("The type {0} cannot be used in table-valued parameter", type));
            }

            return rec;
        }

        //Перманентно (в пределах типа генерика) присваиваем SqlDbType нужный тип данных
        static TableValuedParameter()
        {
            IsObject = typeof(T).GetTypeInfo().IsClass && typeof(T) != typeof(string);

            if (!IsObject)
            {
                SqlDbType = LookupDbType(typeof(T));
            }
        }

        private static Type UnfoldType(Type type)
        {
            // Nullable<T> => T
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);
            // Enum : T => T
            if (type.GetTypeInfo().IsEnum)
                type = Enum.GetUnderlyingType(type);
            return type;
        }

        /// <summary>
        /// Ищем SqlDbType, соответствующий типу, значение которого будет передано в БД.
        /// </summary>
        /// <param name="type">Исходный тип, значение которого будет передано в БД.</param>
        /// <returns>Соответствующий тип БД (SqlDbType).</returns>
        private static SqlDbType LookupDbType(Type type)
        {
            //var type = typeof (T);
            SqlDbType dbType;

            if (TableValuedParameterTypeMap.TypeMap.TryGetValue(UnfoldType(type), out dbType))
            {
                return dbType;
            }
            throw new NotSupportedException(
                string.Format("The type {0} cannot be used in a table-valued parameter", type));
        }

        //Составляем словарь пар "столбец - тип данных" в бд
        private Dictionary<string, SqlDbType> LookupDbTypes()
        {
            if (_columns.Count > 0)
            {
                var dbTypes = new Dictionary<string, SqlDbType>(_columns.Count);
                foreach (var column in _columns)
                {
                    var sourceType = _properties[column].PropertyType;
                    dbTypes[column] = LookupDbType(sourceType);
                }
                return dbTypes;
            }
            throw new ArgumentException(string.Format("Для объекта {0} следует задать отображающие свойства.",
                                                      typeof(T)));
        }

        private IEnumerable<PropertyInfo> GetTypeProperties()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        #endregion
    }
}
