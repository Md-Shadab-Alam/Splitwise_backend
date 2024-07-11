using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Splitwise.Entities;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Splitwise.Services
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName = "data_queue";

        public RabbitMQConsumerService()
        {
            //creating connection Factory 
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //creates a connection to the RabbitMQ server.
            connection = factory.CreateConnection();

            //creating channel, which is a lightweight connection wrapper
            channel = connection.CreateModel();

            //Declare a queue named "data_queue"
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var data = JsonSerializer.Deserialize<Users>(content);

                Console.WriteLine($"Received: {data.Name}");

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Dispose();
            base.Dispose();
        }
    }
}
