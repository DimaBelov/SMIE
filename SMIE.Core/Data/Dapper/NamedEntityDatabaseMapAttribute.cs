using System;

namespace SMIE.Core.Data.Dapper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NamedEntityDatabaseMapAttribute : Attribute
    {
        public NamedEntityDatabaseMapAttribute(string idFieldName, string nameFieldName)
        {
            IdFieldName = idFieldName;
            NameFieldName = nameFieldName;
        }

        public string IdFieldName { get; private set; }
        public string NameFieldName { get; private set; }
    }
}
