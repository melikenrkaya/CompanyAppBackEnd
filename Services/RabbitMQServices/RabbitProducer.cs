using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace companyappbasic.Services.RabbitMQServices
{
    public class RabbitProducer
    {

        private readonly IConfiguration _configuration;

        public RabbitProducer(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task SendMessageAsync(string email, string subject, string body)
        {
            try
            {
                await Task.Run(() =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _configuration["RabbitMQ:HostName"],
                        UserName = _configuration["RabbitMQ:UserName"],
                        Password = _configuration["RabbitMQ:Password"]
                    };

                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: "mailatma",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var messageObject = new
                    {
                        email,
                         subject,
                         body
                    };
                    var message = JsonConvert.SerializeObject(messageObject);
                    var bodyBytes = Encoding.UTF8.GetBytes(message);

                    // RabbitMQ işlemi asenkron olarak yapılmaz, ancak burada await kullanarak diğer asenkron işlere geçiş sağlayabilirsiniz.
                    channel.BasicPublish(exchange: "", routingKey: "mailatma", basicProperties: null, body: bodyBytes);

                    Console.WriteLine("Mesaj kuyruğa gönderildi.");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mesaj gönderme hatası: {ex.Message}");
            }
        }

    }
}

