namespace Transacoes.Application;

public interface IMessageService {
    Task SendMessageAsync(string messageBody);
}