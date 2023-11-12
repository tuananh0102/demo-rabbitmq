using Backend.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Backend.BL
{
    public class SyncBl : ISyncBl
    {
        public void PublishMessage(int num, string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/",
                Port=5672
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

          

           channel.QueueDeclare("task", durable: true, exclusive: false);

            for (int i = 0; i < num; ++i)
            {
                var msg = JsonSerializer.Serialize($"Task number {i + 1}");

                var body = Encoding.UTF8.GetBytes(msg);

            
                channel.BasicPublish("","task",body:body);
            }
        }
    }
}
