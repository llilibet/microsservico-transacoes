using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Transacoes.Application;

namespace Transacoes.Infrastructure;

public class MessageService : IMessageService
{
    private readonly string _connectionString;
    private readonly string _queueName;

    public MessageService(IConfiguration configuration) {

        _connectionString = configuration.GetValue<string>("ServiceBusSettings:ConnectionString") 
            ?? throw new InvalidOperationException("Service Bus ConnectionString não encontrada nas configurações.");
            
        _queueName = configuration.GetValue<string>("ServiceBusSettings:QueueName")
            ?? throw new InvalidOperationException("Service Bus QueueName não encontrado nas configurações.");
    }

    public async Task SendMessageAsync(string messageBody) {
        
        await using var client = new ServiceBusClient(_connectionString);
        ServiceBusSender sender = client.CreateSender(_queueName);
        ServiceBusMessage message = new ServiceBusMessage(messageBody);
        await sender.SendMessageAsync(message);
    }
}