using Microsoft.AspNetCore.Hosting.Server;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Splitwise.Services
{
    public class RabbitMQService 
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string _queueName = "data_queue";


        public RabbitMQService() 
        {
            //creating connection Factory 
            var factory = new ConnectionFactory() { HostName =  "localhost" };

            //creates a connection to the RabbitMQ server.
            connection = factory.CreateConnection();

            //creating channel, which is a lightweight connection wrapper
            channel = connection.CreateModel();

            //Declare a queue named "data_queue"
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.ExchangeDeclare(exchange: "admin-approval", type: ExchangeType.Direct);
        }

        public void SendMessage(object data)
        {
            //Searalize the data to json format
            var json = JsonSerializer.Serialize(data);

            //Convert the JSON string to a byte array
            var body = Encoding.UTF8.GetBytes(json);

            //Publish the message to the queue
            channel.BasicPublish(exchange: "", routingKey: _queueName,basicProperties:null, body: body);
        }

        public void Dispose()
        {
            //close the channel and connection when its done
            channel.Close();
            connection.Close();
        }
        public void ConsumeExpenses (Action<string> callback)
        {
            var consumer = new EventingBasicConsumer(channel); consumer.Received += (model, ea) => {
            var body = ea.Body.ToArray();
            
            var expenseMessage = Encoding.UTF8.GetString(body); callback(expenseMessage);
        }; 
            channel.BasicConsume(queue:"expense-submission", autoAck: true, consumer: consumer);
        }
    }
}
