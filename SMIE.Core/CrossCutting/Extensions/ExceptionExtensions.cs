using System;
using System.Text;

namespace SMIE.Core.CrossCutting.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetMessages(this Exception exception)
        {
            var mes = new StringBuilder();
            while (exception != null)
            {
                mes.AppendLine(exception.Message);
                exception = exception.InnerException;
            }
            return mes.ToString();
        }
    }
}
