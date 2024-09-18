using System.ComponentModel.DataAnnotations;

namespace companyappbasic.Data.Models
{
    

    public class AdminDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;

    }
    public class CreateAdminRequestDto
    {
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
     }

    public class UpdateAdminRequestDto
    {   
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
         }
}
