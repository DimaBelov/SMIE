using SMIE.Core.Data.Dapper;

namespace SMIE.DAL.Entities
{
    public class User
    {
        [DatabaseMap("id")]
        public int Id { get; set; }

        [DatabaseMap("name")]
        public string UserName { get; set; }

        [DatabaseMap("emai")]
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
