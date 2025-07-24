using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json; // Adicione este using
using Transacoes.Application;
using Transacoes.Domain;

namespace Transacoes.Infrastructure;

public class TransactionService : ITransactionService
{
    private readonly IMongoCollection<Transaction> _transactionsCollection;
    private readonly IMessageService _messageService; // Adiciona o serviço de mensagens

    // O construtor agora recebe os dois serviços
    public TransactionService(IOptions<TransactionDatabaseSettings> settings, IMessageService messageService)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _transactionsCollection = mongoDatabase.GetCollection<Transaction>(settings.Value.CollectionName);
        _messageService = messageService; // Guarda a referência
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactionsCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(Transaction newTransaction)
    {
        // salva a transação no banco de dados
        await _transactionsCollection.InsertOneAsync(newTransaction);

        // converte a nova transação para uma string JSON
        string messageBody = JsonSerializer.Serialize(newTransaction);

        // envia a mensagem para a fila do Azure Service Bus
        await _messageService.SendMessageAsync(messageBody);
    }
}