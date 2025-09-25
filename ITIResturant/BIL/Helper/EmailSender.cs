public class EmailSender 
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    /*  public async Task SendEmailAsync(string toEmail, string subject, string message)
      {
          var smtpClient = new SmtpClient("smtp.gmail.com")
          {
              Port = 587,
              Credentials = new NetworkCredential(
                  _config["EmailSettings:Email"],
                  _config["EmailSettings:Password"]),
              EnableSsl = true,
          };

          var mailMessage = new MailMessage
          {
              From = new MailAddress(_config["EmailSettings:Email"], "Restaurant App"),
              Subject = subject,
              Body = message,
              IsBodyHtml = true,
          };

          mailMessage.To.Add(toEmail);

          await smtpClient.SendMailAsync(mailMessage);*/



    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailSettings = _config.GetSection("EmailSettings");
        var fromEmail = emailSettings["SenderEmail"];
        var password = emailSettings["SenderPassword"];

        using var client = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]))
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        var fromAddress = new MailAddress(fromEmail, "Restaurant App");

        var mailMessage = new MailMessage
        {
            From = fromAddress,
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}
