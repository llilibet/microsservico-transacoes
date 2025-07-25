using Transacoes.Domain;

namespace Transacoes.Application;

public interface ITransactionRepository {
    Task CreateAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
}