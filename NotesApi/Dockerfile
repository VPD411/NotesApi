# Этап 1: Сборка
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстановливаем зависимости (отдельно для кеширования)
COPY ["NotesApi.csproj", "./"]
RUN dotnet restore "NotesApi.csproj"

# Копируем все остальные файлы и собираем в папку /app
COPY . .
RUN dotnet publish "NotesApi.csproj" -c Release -o /app/publish

# Этап 2: Запуск
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Создаём папку для данных (SQLite)
RUN mkdir /app/data

# Копируем опубликованное приложение из этапа сборки
COPY --from=build /app/publish .

# Указываем порт, который слушает приложение
ENV PORT = 5000
EXPOSE 5000

# Запускаем приложение
ENTRYPOINT ["dotnet", "NotesApi.dll"]