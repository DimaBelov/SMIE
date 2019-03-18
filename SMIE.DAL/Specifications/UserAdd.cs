using SMIE.Core.Data.Specification;
using SMIE.DAL.Entities;

namespace SMIE.DAL.Specifications
{
    sealed class UserAdd : PgSqlSpecification
    {
        public UserAdd(User user)
        {
            AddParameter("name", user.UserName);
            AddParameter("email", user.Email);
            AddParameter("password", user.Password);
        }

        protected override string CreateCommandText()
        {
            return "select * from user_add(@name, @email, @password)";
        }
    }
}
