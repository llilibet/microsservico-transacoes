# Desafio Backend: Microsserviço de Gestão de Transações (C#)

## Descrição do Projeto

Este é um microsserviço desenvolvido em C# com ASP.NET Core como parte do desafio Talent Lab. O objetivo do projeto é gerir transações financeiras, permitindo a sua criação e listagem através de uma API REST.

O projeto foi desenvolvido seguindo os princípios da **Clean Architecture**, separando as responsabilidades em camadas de Domínio, Aplicação e Infraestrutura. Para persistência de dados, foi utilizado o **MongoDB Atlas**, e para o processamento assíncrono de eventos, o **Azure Service Bus** foi integrado para notificar a criação de novas transações.

## Arquitetura

O projeto segue os princípios da Clean Architecture para garantir um código desacoplado, testável e de fácil manutenção. As camadas são divididas da seguinte forma:

* **Domain:** Contém as entidades de negócio principais (a classe `Transaction`).
* **Application:** Contém as interfaces e a lógica de aplicação (os "casos de uso", como `ITransactionService`).
* **Infrastructure:** Contém as implementações técnicas, como o acesso ao banco de dados (MongoDB) e o serviço de mensagens (Azure Service Bus).
* **API (Presentation):** A camada de entrada do sistema, responsável por expor os endpoints REST utilizando Minimal APIs do ASP.NET Core.

## Tecnologias Utilizadas

* .NET 8 / C#
* ASP.NET Core (com Minimal APIs)
* MongoDB Atlas (Banco de Dados NoSQL)
* Azure Service Bus (Fila de Mensagens)
* xUnit (Framework de Testes)
* Moq (Framework de Mocking para Testes)

## Endpoints Disponíveis

A API expõe os seguintes endpoints:

### Listar Todas as Transações

* **Método:** `GET`
* **Endpoint:** `/transactions`
* **Resposta de Sucesso (200 OK):**
    ```json
    [
      {
        "id": "guid-da-transacao",
        "amount": 150.75,
        "description": "Compra de supermercado",
        "date": "2025-07-25T10:00:00Z"
      }
    ]
    ```

### Criar uma Nova Transação

* **Método:** `POST`
* **Endpoint:** `/transactions`
* **Corpo da Requisição (Request Body):**
    ```json
    {
      "amount": -50.00,
      "description": "Pagamento de conta de luz"
    }
    ```
* **Resposta de Sucesso (201 Created):** Retorna o objeto da transação criada.

## Como Executar o Projeto

### Pré-requisitos

* .NET 8 SDK
* Uma conta no MongoDB Atlas
* Uma conta na Azure com um namespace do Service Bus configurado

### Configuração

1.  Clone este repositório.
2.  Configure as suas "connection strings" no ficheiro `TransacoesAPI/appsettings.json`. Você precisará preencher os valores para `TransactionDatabaseSettings` (MongoDB) e `ServiceBusSettings` (Azure).
3.  Abra a pasta raiz da solução (`transacoes`) no seu terminal.
4.  Execute o comando para restaurar todas as dependências:
   
```bash
dotnet restore
```

### Execução

Para iniciar o microsserviço, execute o seguinte comando a partir da pasta raiz da solução:

```bash
dotnet run --project TransacoesAPI
```

A API estará disponível em `http://localhost:5114` (ou na porta indicada no seu terminal). A documentação interativa do Swagger pode ser acedida em `http://localhost:5114/swagger`.

## Testes

Para rodar os testes unitários, execute o seguinte comando a partir da pasta raiz da solução:

```bash
dotnet test
