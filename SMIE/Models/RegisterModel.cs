﻿using System.ComponentModel.DataAnnotations;

namespace SMIE.Models
{
    public class RegisterModel
    {
        //[Required(ErrorMessage = "Не указан Username")]
        //public string Username { get; set; }

        [Required(ErrorMessage = "No Email specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }
    }
}
