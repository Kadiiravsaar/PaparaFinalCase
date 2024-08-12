namespace Papara.Service.Services.Abstract
{
	public interface IEmailService
	{
		 void SendEmail(string subject, string email, string content);
	}
}
