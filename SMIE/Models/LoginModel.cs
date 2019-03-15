using System.ComponentModel.DataAnnotations;

namespace SMIE.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "No user name (or email) specified")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "A password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
