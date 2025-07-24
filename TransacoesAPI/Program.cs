using Transacoes.Application;
using Transacoes.Domain;
using Transacoes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// configurar a leitura das configurações
builder.Services.Configure<TransactionDatabaseSettings>(
    builder.Configuration.GetSection("TransactionDatabaseSettings"));

// adicionar nosso serviço ao sistema de injeção de dependência
builder.Services.AddSingleton<ITransactionService, TransactionService>();

// adicione esta linha para registrar o novo serviço
builder.Services.AddSingleton<IMessageService, MessageService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GET: /transactions
app.MapGet("/transactions", async (ITransactionService transactionService) =>
{
    var transactions = await transactionService.GetAsync();
    return Results.Ok(transactions);
})
.WithName("GetTransactions")
.WithOpenApi();


// POST: /transactions
app.MapPost("/transactions", async (Transaction newTransaction, ITransactionService transactionService) =>
{
    newTransaction.Id = Guid.NewGuid();
    newTransaction.Date = DateTime.UtcNow;

    await transactionService.CreateAsync(newTransaction);
    return Results.Created($"/transactions/{newTransaction.Id}", newTransaction);
})
.WithName("CreateTransaction")
.WithOpenApi();


app.Run();