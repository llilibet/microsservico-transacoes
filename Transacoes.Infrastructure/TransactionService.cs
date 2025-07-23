using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Transacoes.Application;
using Transacoes.Domain;

namespace Transacoes.Infrastructure;

public class TransactionService : ITransactionService
{
    private readonly IMongoCollection<Transaction> _transactionsCollection;

    public TransactionService(IOptions<TransactionDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _transactionsCollection = mongoDatabase.GetCollection<Transaction>(settings.Value.CollectionName);
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactionsCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(Transaction newTransaction) =>
        await _transactionsCollection.InsertOneAsync(newTransaction);
}