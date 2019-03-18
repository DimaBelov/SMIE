using System;
using System.Collections.Generic;

namespace SMIE.Core.Data.Dapper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DatabaseMapAttribute : Attribute
    {
        public DatabaseMapAttribute(string itemName)
        {
            ItemName = new[] {itemName};
        }

        public IEnumerable<string> ItemName { get; private set; }
    }
}
