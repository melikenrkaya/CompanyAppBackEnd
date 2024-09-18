using companyappbasic.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace companyappbasic.Data.Models
{
    public class LoginDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
    public class RegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string? UserName { get; set; }

        public string? Email { get; set; }


        [Required(ErrorMessage = "Şifre zorunludur")]
        public string? Password { get; set; }
        public string? Role { get; set; }


    }
    public class NewUserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }

    }


}
