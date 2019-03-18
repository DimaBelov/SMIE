using System.Data.SqlClient;

namespace SMIE.Core.CrossCutting.Extensions
{
    public static class SqlExceptionExtension
    {
        public static bool IsDefinedError(this SqlException sqlException)
        {
            return sqlException.Class == 16 && sqlException.Number == 50000;
        }
    }
}
