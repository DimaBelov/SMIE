using System;
using System.Linq;
using System.Reflection;
using Dapper;

namespace SMIE.Core.Data.Dapper
{
    public class ColumnAttributeTypeMapper<T> : FallBackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
            {
                new CustomPropertyTypeMap(typeof (T),
                    (type, columnName) =>
                    {
                        var namedEntityAttribute =
                            type.GetTypeInfo().GetCustomAttribute<NamedEntityDatabaseMapAttribute>();

                        return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .FirstOrDefault(prop =>
                            {
                                if (namedEntityAttribute != null)
                                {
                                    if (string.Equals(prop.Name, "Id", StringComparison.OrdinalIgnoreCase))
                                        return string.Equals(namedEntityAttribute.IdFieldName, columnName,
                                            StringComparison.OrdinalIgnoreCase);

                                    if (string.Equals(prop.Name, "Name", StringComparison.OrdinalIgnoreCase))
                                        return string.Equals(namedEntityAttribute.NameFieldName, columnName,
                                            StringComparison.OrdinalIgnoreCase);

                                    var hierarcyEntityAttribute =
                                        namedEntityAttribute as HierarchyEntityDatabaseMapAttribute;

                                    if (hierarcyEntityAttribute != null &&
                                        string.Equals(prop.Name, "ParentId", StringComparison.OrdinalIgnoreCase))
                                        return string.Equals(hierarcyEntityAttribute.ParentIdFieldName, columnName,
                                            StringComparison.OrdinalIgnoreCase);
                                }

                                return prop.GetCustomAttributes(true)
                                    .OfType<DatabaseMapAttribute>()
                                    .SelectMany(x => x.ItemName)
                                    .Any(
                                        attribute =>
                                            string.Equals(attribute, columnName, StringComparison.OrdinalIgnoreCase));
                            });
                    }),
                new DefaultTypeMap(typeof (T))
            })
        {
        }
    }
}
