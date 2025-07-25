// Transacoes.Infrastructure/TransactionService.cs
using System.Text.Json;
using Transacoes.Application;
using Transacoes.Domain;

namespace Transacoes.Infrastructure;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageService _messageService;

    public TransactionService(ITransactionRepository transactionRepository, IMessageService messageService)
    {
        _transactionRepository = transactionRepository;
        _messageService = messageService;
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactionRepository.GetAllAsync();

    public async Task CreateAsync(Transaction newTransaction)
    {
        await _transactionRepository.CreateAsync(newTransaction);
        string messageBody = JsonSerializer.Serialize(newTransaction);
        await _messageService.SendMessageAsync(messageBody);
    }
}