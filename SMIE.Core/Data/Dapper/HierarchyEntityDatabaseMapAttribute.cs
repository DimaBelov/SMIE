using System;

namespace SMIE.Core.Data.Dapper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HierarchyEntityDatabaseMapAttribute : NamedEntityDatabaseMapAttribute
    {
        public string ParentIdFieldName { get; }

        public HierarchyEntityDatabaseMapAttribute(string idFieldName, string nameFieldName, string parentIdFieldName)
            : base(idFieldName, nameFieldName)
        {
            ParentIdFieldName = parentIdFieldName;
        }
    }
}
