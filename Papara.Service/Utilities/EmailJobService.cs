using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Papara.Service.Services.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Papara.Service.Utilities
{
	public class EmailJobService
	{
		private readonly IConfiguration _configuration;
		private readonly IEmailService _emailService;
		private IModel _channel;
		private IConnection _connection;

		public EmailJobService(IConfiguration configuration, IEmailService emailService)
		{
			_configuration = configuration;
			_emailService = emailService;
			InitializeRabbitMQ();
		}

		private void InitializeRabbitMQ()
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:HostName"],
				UserName = _configuration["RabbitMQ:UserName"],
				Password = _configuration["RabbitMQ:Password"]
			};
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "emails", durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		public void ProcessEmailQueue()
		{
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var email = JsonConvert.DeserializeObject<dynamic>(message);

				string subject = Convert.ToString(email.Subject);
				string emailTo = Convert.ToString(email.Email);
				string content = Convert.ToString(email.Content);



				_emailService.SendEmail(subject, emailTo, content);

			};
			_channel.BasicConsume(queue: "emails", autoAck: true, consumer: consumer);
		}

		public void Dispose()
		{
			_channel?.Close();
			_connection?.Close();
		}
	}



}
