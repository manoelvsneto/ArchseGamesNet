using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Archse.Publisher
{
    public interface IPublisher
    {
        Task Publish<T>(T messagem, string queue);
    }
    public class Publisher : IPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<Publisher> _logger;
        public Publisher(ILogger<Publisher> logger, ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
        }

        public  async Task Publish<T>(T messagem, string queue)
        {
            string jsonString = JsonSerializer.Serialize(messagem);
            await using ServiceBusSender sender = _serviceBusClient.CreateSender(queue);



            try
            {
                

                // Cria uma mensagem para enviar
                ServiceBusMessage message = new ServiceBusMessage(jsonString);
                // Envia a mensagem para a fila
                await sender.SendMessageAsync(message);
                string sended = JsonSerializer.Serialize(message);

                _logger.LogError($"Mensagem enviada para a fila {queue}: {sended}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao enviar mensagem: {ex.Message}");
            }
        }
    }
}