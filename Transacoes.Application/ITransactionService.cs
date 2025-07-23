using Transacoes.Domain;

namespace Transacoes.Application;

public interface ITransactionService
{
    Task<List<Transaction>> GetAsync();
    Task CreateAsync(Transaction newTransaction);
}