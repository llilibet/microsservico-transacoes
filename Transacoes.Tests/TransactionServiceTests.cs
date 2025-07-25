using Moq;
using System.Text.Json;
using Transacoes.Application;
using Transacoes.Domain;
using Transacoes.Infrastructure;

namespace Transacoes.Tests;

public class TransactionServiceTests {
    // declara od 'mocks' (objetos falsos)
    private readonly Mock<ITransactionRepository> _mockRepository;
    private readonly Mock<IMessageService> _mockMessageService;
    
    // declara o 'System Under Test' (SUT), o objeto que estamos a testar
    private readonly ITransactionService _sut;

    // O construtor é executado antes de cada teste, preparando um ambiente limpo
    public TransactionServiceTests() {
        _mockRepository = new Mock<ITransactionRepository>();
        _mockMessageService = new Mock<IMessageService>();
        _sut = new TransactionService(_mockRepository.Object, _mockMessageService.Object);
    }

    [Fact] // A anotação [Fact] marca esse método como um teste a ser executado pelo xUnit
    public async Task CreateAsync_ShouldCallRepositoryAndMessageService_WhenCalled() {
        // Arrange (Organizar)
        // prepara os dados de entrada para o nosso teste
        var newTransaction = new Transaction { 
            Id = Guid.NewGuid(), 
            Amount = 100, 
            Description = "Teste", 
            Date = DateTime.UtcNow 
        };

        // Act (Agir)
        // executa o método que queremos testar
        await _sut.CreateAsync(newTransaction);

        // Assert (Verificar)
        // verifica se o comportamento foi o esperado

        // o repositório foi chamado para criar a transação exatamente uma vez?
        _mockRepository.Verify(repo => repo.CreateAsync(newTransaction), Times.Once);

        // o serviço de mensagens foi chamado para enviar a mensagem exatamente uma vez?
        _mockMessageService.Verify(
            service => service.SendMessageAsync(It.IsAny<string>()), // não nos importa o conteúdo da mensagem, apenas que foi enviada
            Times.Once
        );
    }
}