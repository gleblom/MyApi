# --- Stage 1: build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# копируем csproj и восстанавливаем зависимости
COPY *.csproj .
RUN dotnet restore

# копируем остальные файлы и собираем
COPY . .
RUN dotnet publish -c Release -o /app/publish

# --- Stage 2: runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "MyApi.dll"]
