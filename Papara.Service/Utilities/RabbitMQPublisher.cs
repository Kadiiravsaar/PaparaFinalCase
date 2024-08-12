using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Papara.Core.DTOs.Response;
using RabbitMQ.Client;
using System.Text;
using IModel = RabbitMQ.Client.IModel;

namespace Papara.Service.Utilities
{
	public class RabbitMQPublisher : IDisposable
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string _queueName;

		public RabbitMQPublisher(IConfiguration configuration)
		{
			var factory = new ConnectionFactory()
			{
				HostName = configuration["RabbitMQ:HostName"],
				UserName = configuration["RabbitMQ:UserName"],
				Password = configuration["RabbitMQ:Password"]
			};


			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_queueName = "emails";
			_channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		public void Publish(string message)
		{
			var body = Encoding.UTF8.GetBytes(message);
			_channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
		}

		public void Dispose()
		{
			_channel?.Close();
			_connection?.Close();
		}
	}

}
