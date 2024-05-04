using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.auth
{
    public class ResetPasswordViewModel
    {

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } 
        [Required]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
