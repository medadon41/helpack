using helpack.Data;

namespace helpack.Services;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}