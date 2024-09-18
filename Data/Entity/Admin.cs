namespace companyappbasic.Data.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
    }
}
