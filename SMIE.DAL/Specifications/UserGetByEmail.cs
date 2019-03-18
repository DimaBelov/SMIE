using SMIE.Core.Data.Specification;

namespace SMIE.DAL.Specifications
{
    sealed class UserGetByEmail : PgSqlSpecification
    {
        public UserGetByEmail(string email)
        {
            AddParameter("user_email", email);
        }

        protected override string CreateCommandText()
        {
            return "select * from user_getby_email(@user_email)";
        }
    }
}
