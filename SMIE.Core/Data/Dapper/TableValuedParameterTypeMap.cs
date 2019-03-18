using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

namespace SMIE.Core.Data.Dapper
{
    /// <summary>
    /// Табличный тип данных с одним или несколькими столбцами произвольного типа
    /// </summary>
    internal static class TableValuedParameterTypeMap
    {
        // ReSharper disable StaticFieldInGenericType
        public static readonly Dictionary<Type, SqlDbType> TypeMap;
        // ReSharper restore StaticFieldInGenericType

        // Перманентно присваиваем SqlDbType нужный тип данных
        static TableValuedParameterTypeMap()
        {
            TypeMap = new Dictionary<Type, SqlDbType>
            {
                [typeof (byte)] = SqlDbType.TinyInt,
                [typeof (sbyte)] = SqlDbType.TinyInt,
                [typeof (short)] = SqlDbType.SmallInt,
                [typeof (ushort)] = SqlDbType.SmallInt,
                [typeof (int)] = SqlDbType.Int,
                [typeof (uint)] = SqlDbType.Int,
                [typeof (long)] = SqlDbType.BigInt,
                [typeof (ulong)] = SqlDbType.BigInt,
                [typeof (float)] = SqlDbType.Real,
                [typeof (double)] = SqlDbType.Float,
                [typeof (decimal)] = SqlDbType.Decimal,
                [typeof (bool)] = SqlDbType.Bit,
                [typeof (string)] = SqlDbType.NVarChar,
                [typeof (char)] = SqlDbType.NChar,
                [typeof (Guid)] = SqlDbType.UniqueIdentifier,
                [typeof (DateTime)] = SqlDbType.DateTime2,
                [typeof (DateTimeOffset)] = SqlDbType.DateTimeOffset,
                [typeof (TimeSpan)] = SqlDbType.Time,
                [typeof (byte[])] = SqlDbType.Image,
                [typeof (byte?)] = SqlDbType.TinyInt,
                [typeof (sbyte?)] = SqlDbType.TinyInt,
                [typeof (short?)] = SqlDbType.SmallInt,
                [typeof (ushort?)] = SqlDbType.SmallInt,
                [typeof (int?)] = SqlDbType.Int,
                [typeof (uint?)] = SqlDbType.Int,
                [typeof (long?)] = SqlDbType.BigInt,
                [typeof (ulong?)] = SqlDbType.BigInt,
                [typeof (float?)] = SqlDbType.Real,
                [typeof (double?)] = SqlDbType.Float,
                [typeof (decimal?)] = SqlDbType.Decimal,
                [typeof (bool?)] = SqlDbType.Bit,
                [typeof (char?)] = SqlDbType.NChar,
                [typeof (Guid?)] = SqlDbType.UniqueIdentifier,
                [typeof (DateTime?)] = SqlDbType.DateTime2,
                [typeof (DateTimeOffset?)] = SqlDbType.DateTimeOffset,
                [typeof (TimeSpan?)] = SqlDbType.Time,
                [typeof (SqlBinary)] = SqlDbType.Binary
            };
        }
    }
}
