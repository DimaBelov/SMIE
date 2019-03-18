using System;
using System.Collections.Generic;

namespace SMIE.Core.CrossCutting.Exceptions
{
    public class ErpSqlException : Exception
    {
        public string CommandText { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public ErpSqlException(string commandText, Exception exception) :base(exception.Message, exception)
        {
            CommandText = commandText;
        }

        public override string Message => $"SQL Exception in {CommandText} with params {Parameters}; {base.Message}";
    }
}
