namespace companyappbasic.Services.EmailServices
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
