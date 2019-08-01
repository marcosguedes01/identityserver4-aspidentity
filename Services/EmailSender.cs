using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Grw.Gin.Auth.Services
{
    // https://medium.com/@kevinrodrguez/enabling-email-verification-in-asp-net-core-identity-ui-2-1-b87f028a97e0
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
