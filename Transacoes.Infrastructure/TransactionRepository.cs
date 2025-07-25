using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Transacoes.Application;
using Transacoes.Domain;

namespace Transacoes.Infrastructure;

public class TransactionRepository : ITransactionRepository
{
    private readonly IMongoCollection<Transaction> _transactionsCollection;

    public TransactionRepository(IOptions<TransactionDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _transactionsCollection = mongoDatabase.GetCollection<Transaction>(settings.Value.CollectionName);
    }

    public async Task<List<Transaction>> GetAllAsync() =>
        await _transactionsCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(Transaction transaction) =>
        await _transactionsCollection.InsertOneAsync(transaction);
}