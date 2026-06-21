using Dapper;
using Microsoft.Data.Sqlite;
using NotesApi.Models;

namespace NotesApi.Data;

public class NoteRepository : INoteRepository
{
    private readonly string _connectionString;

    public NoteRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        using var db = GetConnection();
        return await db.QueryAsync<Note>("SELECT * FROM Notes ORDER BY CreatedAt DESC");
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        using var db = GetConnection();
        return await db.QuerySingleOrDefaultAsync<Note>("SELECT * FROM Notes WHERE Id = @Id", new { Id = id });
    }

    public async Task<Note> CreateAsync(Note note)
    {
        using var db = GetConnection();
        var sql = "INSERT INTO Notes(Title, Content, CreatedAt) VALUES (@Title, @Content, @CreatedAt); SELECT last_insert_rowid();";
        note.CreatedAt = DateTime.Now;
        note.Id = await db.QuerySingleAsync<int>(sql, note);
        return note;
    }

    public async Task<bool> UpdateAsync(int id, Note newNote)
    {
        using var db = GetConnection();
        var sql = "UPDATE Notes SET Title = @Title, Content = @Content WHERE Id = @Id";
        var rows = await db.ExecuteAsync(sql, new { newNote.Title, newNote.Content, Id = id });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var db = GetConnection();
        var rows = await db.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }
}