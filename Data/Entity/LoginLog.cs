namespace companyappbasic.Data.Entity
{
    public class LoginLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int NumberOfLogin { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }      
        public DateTime LoginTime { get; set; }
    }

}
