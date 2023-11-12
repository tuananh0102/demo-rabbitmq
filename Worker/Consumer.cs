using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class Consumer
    {

        public void Consume()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/",
                Port = 5672
            };

          
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("task", durable: true, exclusive: false);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

            Random random = new Random();


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var task = new Task(() =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Worker Received {message}");

                    int time = random.Next(1,10);

                    Thread.Sleep(time*1000);

                    Console.WriteLine($"Done {message} in {time}s");

                    // here channel could also be accessed as ((EventingBasicConsumer)sender).Model
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                });
                task.Start();
            };

            channel.BasicConsume(queue: "task",
                     autoAck: false,
                     consumer: consumer);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
