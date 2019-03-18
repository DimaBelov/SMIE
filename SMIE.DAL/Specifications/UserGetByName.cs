using SMIE.Core.Data.Specification;

namespace SMIE.DAL.Specifications
{
    sealed class UserGetByName : PgSqlSpecification
    {
        public UserGetByName(string name)
        {
            AddParameter("user_name", name);
        }

        protected override string CreateCommandText()
        {
            return "select * from user_getby_name(@user_name)";
        }
    }
}
