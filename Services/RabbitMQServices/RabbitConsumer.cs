using companyappbasic.Services.EmailServices;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using companyappbasic.Data.Entity;

namespace companyappbasic.Services.RabbitMQServices
{
    public class RabbitConsumer
    {
       
            private readonly IConfiguration _configuration;
            private readonly IEmail _emailServi;

            public RabbitConsumer(IConfiguration configuration, IEmail emailServi)
            {
                _configuration = configuration;
                _emailServi = emailServi;
            }

            public void StartConsuming()
            {
                try
                {
                    Console.WriteLine("RabbitMQ kuyruğu dinleniyor..");

                    var rabbitMQOptions = _configuration.GetSection("RabbitMQ");
                    var factory = new ConnectionFactory()
                    {
                        HostName = rabbitMQOptions["HostName"],
                        UserName = rabbitMQOptions["UserName"],
                        Password = rabbitMQOptions["Password"],
                        AutomaticRecoveryEnabled = true,
                        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                    };

                     var connection = factory.CreateConnection();
                     var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: "mailatma",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        Console.WriteLine("Mesaj alındı."); 
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(message);

                        if (emailMessage != null)
                        {
                            await _emailServi.SendEmailAsync(emailMessage.Email!, emailMessage.Subject!, emailMessage.Body!);
                        }
                    };
                    channel.BasicConsume(queue: "mailatma", autoAck: true, consumer: consumer);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error: {ex.Message}");
                    throw;
                }
            }
        }
    }
