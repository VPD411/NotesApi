using Dapper;
using Microsoft.Data.Sqlite;

namespace NotesApi.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Notes(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
                )
        ");


        var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Notes");
        if (count == 0)
        {
            connection.Execute(@"
                INSERT INTO Notes(Title, Content, CreatedAt) VALUES 
                ('Первая заметка', 'Привет, Dapper!', @Now),
                ('Вторая заметка', 'Dapper - быстрая штука', @Now)",
                new { Now = DateTime.Now.ToString("o") });
        }
    }
}