using helpack.Data;
using helpack.Misc;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace helpack.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    
    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\recovery.html";
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();
        MailText = MailText.Replace("[username]", mailRequest.UserName).Replace("[password]", mailRequest.Password);
        
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.From.Add(new MailboxAddress("Helpack", "recovery@helpack.net"));
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = "Password recovery";
        
        var builder = new BodyBuilder();
        
        builder.HtmlBody = MailText;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}