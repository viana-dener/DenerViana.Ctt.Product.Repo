# Desafio CTT - Backend .Net

---

## 🚀 Visão Geral

Este repositório contém uma API de backend desenvolvida em .NET para gerenciamento de produtos. A aplicação utiliza MongoDB como banco de dados e segue boas práticas de design, como repositórios, injeção de dependência e arquitetura limpa.

---

## ⚙️ Funcionalidades

- **Gerenciamento de Produtos:**
  - Listagem de produtos.
  - Consulta por ID.
  - Verificação de existência por ID ou descrição.
  - Adição de novos produtos.
- **Middleware Personalizado:**
  - Manipulação de requisições usando `RequestContextMiddleware`.
- **Auditoria e Rastreamento:**
  - Propriedades de auditoria para rastrear alterações em entidades (Ex.: `Audit`).
- **Logs Estruturados:**
  - Implementação de logger com propriedades como `CorrelationId`.

---

## 🛠️ Arquitetura

A estrutura do projeto segue os princípios de separação de responsabilidades:

- **Api.Domain**: Contém interfaces e classes que definem a lógica de negócios.
- **Api.Infra**: Implementações de persistência de dados e comunicação com o MongoDB.
- **Api.Application**: Serviços de aplicação que conectam o domínio e a infraestrutura.
- **Api.Base**: Classes base para entidades, endpoints e bootstrap de serviços.

---

## 🔧 Tecnologias Utilizadas

- **.NET Core**: Framework para desenvolvimento de APIs.
- **MongoDB**: Banco de dados NoSQL.
- **Serilog**: Biblioteca de logging.
- **AutoMapper**: Para mapeamento de objetos.

---

## 🛠️ Configuração do Ambiente

### Pré-requisitos

- **.NET Core SDK** instalado.
- **MongoDB** configurado e rodando.

### Passos para Configuração

1. Clone o repositório:
git clone https://github.com/viana-dener/DenerViana.Ctt.Product.Repo.git

2. Navegue até a pasta do projeto:
cd DenerViana.Ctt.Product.Repo

3. Restaure os pacotes:
dotnet restore

4. Configure a string de conexão do MongoDB no arquivo `appsettings.json`.

5. Execute a aplicação:
dotnet run


---

## 📄 Endpoints da API

### Produtos
- `GET /products` - Retorna todos os produtos.
- `GET /products/{id}` - Retorna o produto pelo ID.
- `POST /products` - Adiciona um novo produto.

---


## 🧪 Testes

Listando produtos:

![image](https://github.com/user-attachments/assets/05c675ae-e4ca-46cc-ae3a-c0e9cba09dc2)


Obtendo produto por id:

![image](https://github.com/user-attachments/assets/c00896ad-6c87-453d-b005-6a44e42cfab6)

Inserindo um novo produto:

![image](https://github.com/user-attachments/assets/553ef636-3114-463b-8f5e-ca69f548f1d3)

Testes de unidade:

![image](https://github.com/user-attachments/assets/5df8ce70-f502-422d-bfdc-8f8683f1fe04)


---

## 📜 Licença

Este projeto está licenciado sob os termos da licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
