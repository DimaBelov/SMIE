using System.ComponentModel.DataAnnotations;

namespace SMIE.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "No Email specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
