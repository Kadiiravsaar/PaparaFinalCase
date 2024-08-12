using Microsoft.Extensions.Configuration;
using Papara.Service.Services.Abstract;
using System.Net.Mail;

namespace Papara.Service.Services.Concrete
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public void SendEmail(string subject, string email, string content)
		{

			var smtpConfig = _configuration.GetSection("Smtp");
			SmtpClient mySmtpClient = new SmtpClient(smtpConfig["Host"])
			{
				Port = int.Parse(smtpConfig["Port"]),
				Credentials = new System.Net.NetworkCredential(smtpConfig["Username"], smtpConfig["Password"]),
				EnableSsl = true,
			};

			MailAddress from = new MailAddress(smtpConfig["FromEmail"], smtpConfig["FromName"]);
			MailAddress to = new MailAddress(email);
			MailMessage myMail = new MailMessage(from, to)
			{
				Subject = subject,
				SubjectEncoding = System.Text.Encoding.UTF8,
				Body = content,
				BodyEncoding = System.Text.Encoding.UTF8,
				IsBodyHtml = true
			};

			mySmtpClient.Send(myMail);
		}
	}


}
