# Etapa 1: Construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todos os arquivos do projeto para o container
COPY . ./

# Restaurar as dependências
RUN dotnet restore

# Publicar a aplicação em modo Release
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar a aplicação publicada da etapa anterior
COPY --from=build /app/publish .

# Expor a porta usada pela aplicação
EXPOSE 5000
EXPOSE 5001

# Configurar o comando de inicialização
ENTRYPOINT ["dotnet", "DenerViana.Ctt.Product.Api.dll"]